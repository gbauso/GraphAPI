using MongoDB.Bson.Serialization.Attributes;

namespace Graph.Infrastructure.Database.Query.ProjectSchema
{
    public class ProjectTask
    {
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("responsible")]
        public string Responsible { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }
    }
}
