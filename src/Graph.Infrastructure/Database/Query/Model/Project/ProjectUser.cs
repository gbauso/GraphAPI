using MongoDB.Bson.Serialization.Attributes;

namespace Graph.Infrastructure.Database.Query.ProjectSchema
{
    public class ProjectUser
    {
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
