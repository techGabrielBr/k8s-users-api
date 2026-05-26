namespace UsersAPI.Domain.Entities
{
    public class User
    {
        protected User() { } // EF Core

        public User(
                string name,
                string email,
                string passwordHash,
                string role
            )
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Role = role;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public string Role { get; set; } = "User";

        public void SetPassword(string hash)
        {
            PasswordHash = hash;
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
