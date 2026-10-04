using System.Text.Json.Serialization;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;

///<summary>
///       The user aggregate.
/// </summary>
/// <remarks>
///      This class is used to represent a user.
/// </remarks>
public partial class User(Email emailAddress, string passwordHash)
{
    public User() : this(null!, string.Empty)
    {
    }
    
    public string? FullName { get; private set; }
    public void SetFullName(string name) => FullName = name.Trim();
    public int Id { get; }
    public Email EmailAddress { get; private set; } = emailAddress;
    
    [JsonIgnore]
    public string PasswordHash { get; private set; } = passwordHash;

    /// <summary>
    ///     Update Email
    /// </summary>
    /// <param name="newEmail">
    ///     The user new Email
    /// </param>
    /// <returns>
    ///     The user's Email updated
    /// </returns>
    public User UpdateEmail(Email newEmail)
    {
        EmailAddress = newEmail;
        return this;
    }

    /// <summary>
    ///     Update password hash.
    /// </summary>
    /// <param name="newPasswordHash">
    ///     The user new password.
    /// </param>
    /// <returns>
    ///     The user's password updated.
    /// </returns>
    public User UpdatePasswordHash(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        return this;
    }
}