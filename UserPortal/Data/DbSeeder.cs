using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using UserPortal.Entities;

namespace UserPortal.Data
{
    public class DbSeeder
    {
        private readonly IMongoCollection<Role> _roleCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DbSeeder(IMongoDatabase database, IPasswordHasher<User> passwordHasher)
        {
            _roleCollection = database.GetCollection<Role>("Roles");
            _userCollection = database.GetCollection<User>("Users");
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (!_roleCollection.Find(_ => true).Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "User" },
                    new Role { Name = "Admin" }
                };

                _roleCollection.InsertMany(roles);
            }
            if (!_userCollection.Find(_ => true).Any())
            {
                var admin = new User
                {
                    Email = "admin@admin.com",
                    FirstName = "admin",
                    LastName = "admin",
                    RoleId = _roleCollection.Find(r => r.Name == "Admin").FirstOrDefault().Id
                };
                admin.PasswordHash = _passwordHasher.HashPassword(admin, "admin");
                _userCollection.InsertOne(admin);
            }
        }
    }
}
