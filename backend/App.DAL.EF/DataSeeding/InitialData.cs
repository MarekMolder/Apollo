namespace App.DAL.EF.DataSeeding;

/// <summary>
/// Defines initial roles and users for identity seeding.
/// </summary>
public static class InitialData
{
    /// <summary>
    /// List of roles to be created during seeding. Tuple: (RoleName, OptionalId)
    /// </summary>
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("admin", null),
        ];

    /// <summary>
    /// List of users to be created during seeding.
    /// Tuple: (Email/Username, FirstName, LastName, Password, OptionalId, Roles)
    /// </summary>
    public static readonly (string name, string firstName, string lastName, string password, Guid? id, string[] roles)[]
        Users =
        [
            ("admin@taltech.ee", "admin", "taltech", "Foo.Bar.1", null, ["admin"]),
            ("user@taltech.ee", "user", "taltech", "Foo.Bar.2", null, []),
        ];
}