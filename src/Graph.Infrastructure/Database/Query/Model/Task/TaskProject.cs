using MongoDB.Bson.Serialization.Attributes;

namespace Graph.Infrastructure.Database.Query.TaskSchema
{
    public class TaskProject
    {
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
