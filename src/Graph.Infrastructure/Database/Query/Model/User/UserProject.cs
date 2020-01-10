using MongoDB.Bson.Serialization.Attributes;

namespace Graph.Infrastructure.Database.Query.UserSchema
{
    public class UserProject
    {
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("longDescription")]
        public string LongDescription { get; set; }
    }
}
