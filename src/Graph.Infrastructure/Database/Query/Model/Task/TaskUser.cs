using MongoDB.Bson.Serialization.Attributes;

namespace Graph.Infrastructure.Database.Query.TaskSchema
{
    public class TaskUser
    {
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
