using MongoDB.Bson.Serialization.Attributes;
using PK.DB.Utilities.Interfaces;

namespace PK.DB.Utilities.Entities {
    /// <summary>
    /// MongoDb实体抽象基类
    /// </summary>
    public abstract class MongoDbEntity : IMongoDbEntity {
        /// <summary>
        /// 用于绑定MongoDb文档的_id字段
        /// </summary>
        [BsonId]
        public string BsonId { get; set; }
    }
}
