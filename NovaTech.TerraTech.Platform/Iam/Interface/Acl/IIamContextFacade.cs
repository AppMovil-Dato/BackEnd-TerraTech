namespace NovaTech.TerraTech.Platform.Iam.Interface.Acl;
public interface IIamContextFacade
{
    Task<int> CreateUser(string email, string password, CancellationToken cancellationToken);
    Task<int> FetchUserByEmail(string email, CancellationToken cancellationToken);
    Task<string> FetchEmailByUserId(int usedId, CancellationToken cancellationToken);
}
