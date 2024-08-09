using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserPortal.Entities
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
