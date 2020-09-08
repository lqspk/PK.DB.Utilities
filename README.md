# PK.DB.Utilities
C# 通用数据库操作类

MongoDb数据库操作示例：

1.定义一个实体类：

    public class User : IMongoDbEntity {
        [BsonId]
        public string BsonId { get; set; }

        public DateTime CreateTime { get; set; }
        
    }


2.实例化数据库操作类：

    MongoDbHelper dbHelper = new MongoDbHelper(new MongoUrl("mongodb://localhost:27017/Demo"));


3.插入一个文档：

    User user = new User()
    {
        BsonId = Guid.NewGuid().ToString("N"),
        CreateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
    };

    dbHelper.Insert(user);