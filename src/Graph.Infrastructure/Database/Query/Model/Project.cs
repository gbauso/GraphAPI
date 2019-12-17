using Graph.CrossCutting.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Graph.Infrastructure.Database.Query.ProjectSchema
{
    public class Project : IQueryModel
    {
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("longDescription")]
        public string LongDescription { get; set; }

        [BsonElement("tasks")]
        public ICollection<ProjectTask> Tasks { get; set; }

        [BsonElement("participants")]
        public ICollection<ProjectUser> Participants { get; set; }

        [BsonElement("finishedCount")]
        public int FinishedCount { get; set; }

        [BsonElement("unfinishedCount")]
        public int UnfinishedCount { get; set; }
    }

    public class ProjectUser
    {
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

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
