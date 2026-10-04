using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NovaTech.TerraTech.Platform.Migrations
{
    /// <inheritdoc />
    public partial class Tb1Journey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "users",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "profiles",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "size_m2",
                table: "profiles",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "owner_user_id",
                table: "inventories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "crop_name",
                table: "fields",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "devices",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sensor_code",
                table: "devices",
                type: "varchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "sensor_catalog_items",
                columns: table => new
                {
                    sensor_code = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false),
                    mac_address = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    is_demo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensor_catalog_items", x => x.sensor_code);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            // Preserve legacy associations and MACs. Unique IDs map injectively to six base-36 digits.
            migrationBuilder.Sql("INSERT INTO sensor_catalog_items (sensor_code, mac_address, is_demo) SELECT CONCAT('TT-', LPAD(CONV(id, 10, 36), 6, '0')), mac_address, 0 FROM devices");
            migrationBuilder.Sql("UPDATE devices SET sensor_code = CONCAT('TT-', LPAD(CONV(id, 10, 36), 6, '0')) WHERE sensor_code IS NULL");

            migrationBuilder.CreateTable(
                name: "sensor_readings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    device_id = table.Column<int>(type: "int", nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    moisture_percent = table.Column<double>(type: "double", nullable: false),
                    soil_temperature_c = table.Column<double>(type: "double", nullable: false),
                    nitrogen_ppm = table.Column<double>(type: "double", nullable: false),
                    phosphorus_ppm = table.Column<double>(type: "double", nullable: false),
                    potassium_ppm = table.Column<double>(type: "double", nullable: false),
                    source = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensor_readings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_sensor_readings_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_reports_device_id",
                table: "reports",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_profiles_user_id",
                table: "profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_orders_profile_id",
                table: "orders",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_notifications_profile_id",
                table: "notifications",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_inventories_owner_user_id",
                table: "inventories",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_fields_profile_id",
                table: "fields",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_field_id",
                table: "devices",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_mac_address",
                table: "devices",
                column: "mac_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_devices_sensor_code",
                table: "devices",
                column: "sensor_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_community_profiles_profile_id",
                table: "community_profiles",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_comments_author_profile_id",
                table: "comments",
                column: "author_profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_comments_target_profile_id",
                table: "comments",
                column: "target_profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sensor_catalog_items_mac_address",
                table: "sensor_catalog_items",
                column: "mac_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sensor_readings_device_id_recorded_at",
                table: "sensor_readings",
                columns: new[] { "device_id", "recorded_at" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "f_k_comments__profile_author_profile_id",
                table: "comments",
                column: "author_profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_comments__profile_target_profile_id",
                table: "comments",
                column: "target_profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_community_profiles__profile_profile_id",
                table: "community_profiles",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_devices__sensor_catalog_item_sensor_code",
                table: "devices",
                column: "sensor_code",
                principalTable: "sensor_catalog_items",
                principalColumn: "sensor_code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_devices_fields_field_id",
                table: "devices",
                column: "field_id",
                principalTable: "fields",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_fields__profile_profile_id",
                table: "fields",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_inventories_users_owner_user_id",
                table: "inventories",
                column: "owner_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_notifications__profile_profile_id",
                table: "notifications",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_orders__profile_profile_id",
                table: "orders",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_profiles_users_user_id",
                table: "profiles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_reports__device_device_id",
                table: "reports",
                column: "device_id",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_comments__profile_author_profile_id",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_comments__profile_target_profile_id",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_community_profiles__profile_profile_id",
                table: "community_profiles");

            migrationBuilder.DropForeignKey(
                name: "f_k_devices__sensor_catalog_item_sensor_code",
                table: "devices");

            migrationBuilder.DropForeignKey(
                name: "f_k_devices_fields_field_id",
                table: "devices");

            migrationBuilder.DropForeignKey(
                name: "f_k_fields__profile_profile_id",
                table: "fields");

            migrationBuilder.DropForeignKey(
                name: "f_k_inventories_users_owner_user_id",
                table: "inventories");

            migrationBuilder.DropForeignKey(
                name: "f_k_notifications__profile_profile_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "f_k_orders__profile_profile_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "f_k_profiles_users_user_id",
                table: "profiles");

            migrationBuilder.DropForeignKey(
                name: "f_k_reports__device_device_id",
                table: "reports");

            migrationBuilder.DropTable(
                name: "sensor_catalog_items");

            migrationBuilder.DropTable(
                name: "sensor_readings");

            migrationBuilder.DropIndex(
                name: "i_x_reports_device_id",
                table: "reports");

            migrationBuilder.DropIndex(
                name: "i_x_profiles_user_id",
                table: "profiles");

            migrationBuilder.DropIndex(
                name: "i_x_orders_profile_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "i_x_notifications_profile_id",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "i_x_inventories_owner_user_id",
                table: "inventories");

            migrationBuilder.DropIndex(
                name: "i_x_fields_profile_id",
                table: "fields");

            migrationBuilder.DropIndex(
                name: "i_x_devices_field_id",
                table: "devices");

            migrationBuilder.DropIndex(
                name: "i_x_devices_mac_address",
                table: "devices");

            migrationBuilder.DropIndex(
                name: "i_x_devices_sensor_code",
                table: "devices");

            migrationBuilder.DropIndex(
                name: "i_x_community_profiles_profile_id",
                table: "community_profiles");

            migrationBuilder.DropIndex(
                name: "i_x_comments_author_profile_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "i_x_comments_target_profile_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "location",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "size_m2",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "owner_user_id",
                table: "inventories");

            migrationBuilder.DropColumn(
                name: "crop_name",
                table: "fields");

            migrationBuilder.DropColumn(
                name: "name",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "sensor_code",
                table: "devices");
        }
    }
}
