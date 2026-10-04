#!/usr/bin/env python3
"""Explicit local Development demo. Creates a temporary account; never prints credentials."""
import argparse
import json
import os
from pathlib import Path
import secrets
import subprocess
import urllib.error
import urllib.parse
import urllib.request
import uuid

root = Path(__file__).resolve().parents[1]
parser = argparse.ArgumentParser()
parser.add_argument('--base-url', default='http://localhost:8080')
parser.add_argument('--output', type=Path, default=root / 'docs' / 'http-validation.json')
args = parser.parse_args()
if os.environ.get('ASPNETCORE_ENVIRONMENT') != 'Development':
    raise SystemExit('Set ASPNETCORE_ENVIRONMENT=Development explicitly.')
if urllib.parse.urlparse(args.base_url).hostname not in ('localhost', '127.0.0.1', '::1'):
    raise SystemExit('This demo only targets a local isolated backend.')
dotnet = str(Path(os.environ['DOTNET_ROOT']) / 'dotnet') if 'DOTNET_ROOT' in os.environ else 'dotnet'
project = root / 'NovaTech.TerraTech.Platform'
steps = []
token = None


def command(*arguments):
    result = subprocess.run([dotnet, 'run', '--project', str(project), '-c', 'Debug', '--no-build',
                             '--no-launch-profile', '--', *arguments], capture_output=True, text=True)
    if result.returncode:
        raise SystemExit(result.stderr[-1000:] or result.stdout[-1000:])


def request(method, route, body=None, expected=200):
    headers = {'Content-Type': 'application/json'}
    if token:
        headers['Authorization'] = 'Bearer ' + token
    req = urllib.request.Request(args.base_url.rstrip('/') + route,
                                 data=json.dumps(body).encode() if body is not None else None,
                                 headers=headers, method=method)
    try:
        response = urllib.request.urlopen(req)
    except urllib.error.HTTPError as error:
        response = error
    status = response.code
    data = json.loads(response.read())
    steps.append({'method': method, 'route': route, 'status': status, 'expected': expected})
    if status != expected:
        raise SystemExit(f'{method} {route}: expected {expected}, got {status}; {data.get("title", "")}')
    return data


command('--demo-catalog')
email = 'tb1-' + uuid.uuid4().hex + '@example.test'
password = secrets.token_urlsafe(24)
request('POST', '/api/v1/authentication/sign-up',
        {'fullName': 'TB1 Demo Farmer', 'emailAddress': email, 'password': password, 'confirmPassword': password + 'different'}, 400)
user = request('POST', '/api/v1/authentication/sign-up',
               {'fullName': 'TB1 Demo Farmer', 'emailAddress': email, 'password': password, 'confirmPassword': password}, 201)
login = request('POST', '/api/v1/authentication/sign-in', {'emailAddress': email, 'password': password})
token = login['token']
request('GET', '/api/v1/users/me')
request('GET', '/api/v1/profiles/me', expected=404)
profile = request('PUT', '/api/v1/profiles/me', {'fullName': 'TB1 Demo Farmer', 'fundoName': 'Demo Norte',
                   'contactPhone': '999888777', 'location': 'Huaral, Lima', 'sizeM2': 10000})
field = request('POST', '/api/v1/fields', {'profileId': profile['id'], 'name': 'Parcela Demo', 'sizeM2': 5000,
                   'soilType': 'Franco', 'latitude': -11.5, 'longitude': -77.2, 'cropName': 'Papa'}, 201)
parcels = request('GET', '/api/v1/fields')
assert any(parcel['id'] == field['id'] for parcel in parcels)
# Choose a demo sensor not yet claimed. Occupied identities are never detached automatically.
for index in range(1, 6):
    req = urllib.request.Request(args.base_url.rstrip('/') + '/api/v1/devices/register',
          data=json.dumps({'sensorCode': f'TT-ZZZ{index:03}', 'fieldId': field['id'], 'name': 'Sensor Demo'}).encode(),
          headers={'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token}, method='POST')
    try:
        response = urllib.request.urlopen(req)
        device = json.loads(response.read())
        steps.append({'method': 'POST', 'route': '/api/v1/devices/register', 'status': response.code, 'expected': 201})
        break
    except urllib.error.HTTPError as error:
        if error.code != 409:
            raise
else:
    raise SystemExit('All five demo sensors are occupied. Use another isolated database or provision another demo identity.')
devices = request('GET', f'/api/v1/fields/{field["id"]}/devices')
assert len(devices) == 1 and devices[0]['id'] == device['id']
request('GET', f'/api/v1/devices/{device["id"]}/readings/latest', expected=404)
command('--demo-readings', '--device-id', str(device['id']))
latest = request('GET', f'/api/v1/devices/{device["id"]}/readings/latest')
week = request('GET', f'/api/v1/devices/{device["id"]}/readings?days=7')
month = request('GET', f'/api/v1/devices/{device["id"]}/readings?days=30')
detail = request('GET', f'/api/v1/devices/{device["id"]}/readings/{latest["reading"]["id"]}')
assert detail == latest['reading']
assert latest['reading']['source'] == 'SIMULATED'
assert len(month['readings']) == 720
report = {'baseUrl': args.base_url, 'steps': steps, 'userId': user['id'], 'profileId': profile['id'],
          'fieldId': field['id'], 'deviceId': device['id'], 'sensorCode': device['sensorCode'], 'readingId': detail['id'],
          'confirmationValidated': True, 'parcelSelectionValidated': True, 'detailMatchesCachedResource': True,
          'weekReadings': len(week['readings']), 'monthReadings': len(month['readings']),
          'source': latest['reading']['source'], 'latestRecordedAt': latest['reading']['recordedAt'],
          'isStale': latest['isStale'], 'credentialsRecorded': False}
args.output.parent.mkdir(parents=True, exist_ok=True)
args.output.write_text(json.dumps(report, indent=2) + '\n')
print(json.dumps({'stepsPassed': len(steps), 'monthReadings': len(month['readings']), 'report': str(args.output)}))
