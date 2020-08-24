using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PK.DB.Utilities.Interfaces
{
    /// <summary>
    /// MongoDb实体类接口
    /// </summary>
    public interface IMongoDbEntity
    {
        [BsonId] string Id { get; set; }
    }
}
