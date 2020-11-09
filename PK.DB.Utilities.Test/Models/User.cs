using MongoDB.Bson.Serialization.Attributes;
using PK.DB.Utilities.MongoDb;
using System;

namespace PK.DB.Utilities.Test.Models {
    public class User : IMongoDbEntity {
        [BsonId]
        public string BsonId { get; set; }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }

    }
}
