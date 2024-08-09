using MongoDB.Driver;
using UserPortal.Entities;

namespace UserPortal.Data
{
    public class DbSeeder
    {
        private readonly IMongoCollection<Role> _roleCollection;

        public DbSeeder(IMongoDatabase database)
        {
            _roleCollection = database.GetCollection<Role>("Roles");
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
        }
    }
}
