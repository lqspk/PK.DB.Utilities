using MongoDB.Bson.Serialization.Attributes;

namespace PK.DB.Utilities.MongoDb {
    /// <summary>
    /// MongoDb实体抽象基类
    /// </summary>
    public abstract class MongoDbEntity : IMongoDbEntity {
        /// <summary>
        /// 用于绑定MongoDb文档的_id字段
        /// </summary>
        [BsonId]
        public abstract string BsonId { get; set; }
    }
}
