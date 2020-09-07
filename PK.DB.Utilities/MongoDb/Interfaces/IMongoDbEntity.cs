namespace PK.DB.Utilities.MongoDb {
    /// <summary>
    /// MongoDb实体类接口
    /// </summary>
    public interface IMongoDbEntity
    {
        /// <summary>
        /// 用于绑定MongoDb文档的_id字段
        /// </summary>
        string BsonId { get; set; }
    }
}
