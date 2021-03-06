﻿# PK.DB.Utilities
数据库通用操作类，目前支持MongoDB、Oracle数据库。



**MongoDb数据库操作示例：**

1.定义一个实体类：

```c#
public class User : IMongoDbEntity {
    [BsonId]
    public string BsonId { get; set; }

    public DateTime CreateTime { get; set; }
    
}
```


2.实例化数据库操作类：

    MongoDbHelper dbHelper = new MongoDbHelper(new MongoUrl("mongodb://localhost:27017/Demo"));


3.插入一个文档：

```c#
User user = new User()
{
    BsonId = Guid.NewGuid().ToString("N"),
    CreateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
};

dbHelper.Insert(user);
```

4.更新一个文档：

```c#
dbHelper.UpdateOne<User>(s => s.BsonId == "XXXXXXXXXXX", new []
{
    new UpdateField<User>(){ Field = s => s.CreateTime, Value = DateTime.UtcNow },
    new UpdateField<User>(){ Field = s => s.Name, Value = "Li" },
});  
```

5.删除多个文档：

```c#
dbHelper.DeleteMany<User>(s => s.BsonId == "XXXXXXXXXXX");  
```



**Oracle数据库操作示例：**

1.实例化Oralce数据库操作类：

```c#
OracleManagedDataAccessHelper helper = new OracleManagedDataAccessHelper("Data Source=oracle;User Id=oracleuser;Password=pwd");
```

​		

2.执行查询语句：

```c#
var dt = helper.ExecuteQuerySql("select * from v$version");
```

