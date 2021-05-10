using MongoDB.Bson.Serialization.Attributes;
using PK.DB.Utilities.MongoDb;
using System;

namespace PK.DB.Utilities.Test.Models {
    public class UserExt : IMongoDbEntity {
        [BsonId]
        public string BsonId { get; set; }

        public string UserId { get; set; }

        public string IDCode { get; set; }

    }
}
