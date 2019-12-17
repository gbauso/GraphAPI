using Graph.CrossCutting.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Graph.Infrastructure.Database.Query.UserSchema
{
    public class User : IQueryModel
    {
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("projects")]
        public IEnumerable<UserProject> Projects { get; set; }
    }

    public class UserProject
    {
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("longDescription")]
        public string LongDescription { get; set; }
    }
}
