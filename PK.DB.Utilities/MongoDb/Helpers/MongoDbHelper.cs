using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace PK.DB.Utilities.MongoDb
{
    /// <summary>
    /// MongoDb帮助类
    /// </summary>
    public class MongoDbHelper
    {
        /// <summary>
        /// 数据库服务器地址
        /// </summary>
        private string _server;

        /// <summary>
        /// 数据库名称
        /// </summary>
        private string _databaseName;

        /// <summary>
        /// 数据库连接
        /// </summary>
        private readonly IMongoDatabase _db = null;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public IMongoDatabase Database
        {
            get => this._db;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">带数据库名的MongoDb连接地址</param>
        public MongoDbHelper(MongoUrl url)
        {
            this._databaseName = url.DatabaseName;

            if (string.IsNullOrWhiteSpace(this._databaseName))
                throw new Exception("DatabaseName is null or empty.");

            var client = new MongoClient(url);
            this._db = client.GetDatabase(this._databaseName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="server">数据库服务器地址</param>
        /// <param name="databaseName">数据库名称</param>
        public MongoDbHelper(string server, string databaseName)
        {
            this._server = server;
            this._databaseName = databaseName;

            if (string.IsNullOrWhiteSpace(this._databaseName))
                throw new Exception("DatabaseName is null or empty.");

            var client = new MongoClient(server);

            this._db = client.GetDatabase(this._databaseName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings">设置参数</param>
        public MongoDbHelper(MongoClientSettings settings)
        {
            if (settings == null)
                throw new Exception("settings is null.");

            var client = new MongoClient(settings);

            this._db = client.GetDatabase(this._databaseName);
        }


        /// <summary>
        /// 获取IMongoCollection
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : IMongoDbEntity
        {
            return this._db.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        /// <summary>
        /// 新增一个文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns> 
        public void Insert<TDocument>(TDocument entity) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            this._db.GetCollection<TDocument>(type.Name).InsertOne(entity);
        }

        /// <summary>
        /// 异步新增一个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync<TDocument>(TDocument entity) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            await this._db.GetCollection<TDocument>(type.Name).InsertOneAsync(entity);
        }

        /// <summary>
        /// 批量新增文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entities"></param>
        public void InsertMany<TDocument>(IList<TDocument> entities) where TDocument : IMongoDbEntity
        {
            if (entities != null)
            {
                var entity = entities.FirstOrDefault();

                var type = CheckHasBsonIdAttribute<TDocument>();

                this._db.GetCollection<TDocument>(type.Name).InsertMany(entities);
            }
        }

        /// <summary>
        /// 异步批量新增文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entities"></param>
        public async Task InsertManyAsync<TDocument>(IList<TDocument> entities) where TDocument : IMongoDbEntity
        {
            if (entities != null)
            {
                var entity = entities.FirstOrDefault();

                var type = CheckHasBsonIdAttribute<TDocument>();

                await this._db.GetCollection<TDocument>(type.Name).InsertManyAsync(entities);
            }
        }

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public ReplaceOneResult ReplaceOne<TDocument>(TDocument entity, ReplaceOptions options = null) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return this._db.GetCollection<TDocument>(type.Name)
                .ReplaceOne(s => s.BsonId == entity.BsonId, entity, options);
        }

        /// <summary>
        /// 异步替换单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> ReplaceOneAsync<TDocument>(TDocument entity, ReplaceOptions options = null) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return await this._db.GetCollection<TDocument>(type.Name)
                .ReplaceOneAsync(s => s.BsonId == entity.BsonId, entity, options);
        }

        /// <summary>
        /// 删除单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DeleteResult DeleteOne<TDocument>(TDocument entity) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return this._db.GetCollection<TDocument>(type.Name)
                .DeleteOne(s => s.BsonId == entity.BsonId);
        }

        /// <summary>
        /// 异步删除单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteOneAsync<TDocument>(TDocument entity) where TDocument : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.BsonId))
            {
                throw new Exception("BsonId is null or empty.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return await this._db.GetCollection<TDocument>(type.Name)
                .DeleteOneAsync(s => s.BsonId == entity.BsonId);
        }

        /// <summary>
        /// 删除多个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DeleteResult DeleteMany<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("whereExpression is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return this._db.GetCollection<TDocument>(type.Name)
                .DeleteMany(whereExpression);
        }

        /// <summary>
        /// 异步删除多个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("expression is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            return await this._db.GetCollection<TDocument>(type.Name)
                .DeleteManyAsync(whereExpression);
        }

        /// <summary>
        /// 查找单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDocument FindOne<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            var type = CheckHasBsonIdAttribute<TDocument>();

            return this._db.GetCollection<TDocument>(type.Name)
                    .AsQueryable()
                    .Where<TDocument>(whereExpression)
                    .FirstOrDefault();
        }

        /// <summary>
        /// 异步查找单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TDocument> FindOneAsync<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            var type = CheckHasBsonIdAttribute<TDocument>();
            return await this._db.GetCollection<TDocument>(type.Name)
                .AsQueryable()
                .Where<TDocument>(whereExpression)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查找文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IMongoQueryable<TDocument> Find<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            var type = CheckHasBsonIdAttribute<TDocument>();
            return this._db.GetCollection<TDocument>(type.Name)
                .AsQueryable()
                .Where<TDocument>(whereExpression);
        }

        /// <summary>
        /// 查找文档列表
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<TDocument> FindList<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            var type = CheckHasBsonIdAttribute<TDocument>();
            return this._db.GetCollection<TDocument>(type.Name)
                .AsQueryable()
                .Where<TDocument>(whereExpression)
                .ToList();
        }

        /// <summary>
        /// 异步查找文档列表
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IList<TDocument>> FindListAsync<TDocument>(Expression<Func<TDocument, bool>> whereExpression) where TDocument : IMongoDbEntity
        {
            var type = CheckHasBsonIdAttribute<TDocument>();
            return await this._db.GetCollection<TDocument>(type.Name)
                .AsQueryable()
                .Where<TDocument>(whereExpression)
                .ToListAsync();
        }

        /// <summary>
        /// 更新单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="updateFields">更新的字段和值</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public UpdateResult UpdateOne<TDocument>(Expression<Func<TDocument, bool>> whereExpression, UpdateField<TDocument>[] updateFields, UpdateOptions options = null) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("whereExpression is null.");
            }

            if (updateFields == null || !updateFields.Any())
            {
                throw new Exception("updateFields is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            var updateDefinitionBuilder = Builders<TDocument>.Update;
            var updateDefinitions =
                updateFields.Select(s => updateDefinitionBuilder.Set(s.Field, s.Value)).ToList();

            return this._db.GetCollection<TDocument>(type.Name)
                .UpdateOne(whereExpression, updateDefinitionBuilder.Combine(updateDefinitions), options);
        }

        /// <summary>
        /// 异步更新单个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="updateFields">更新字段和值</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateOneAsync<TDocument>(Expression<Func<TDocument, bool>> whereExpression, UpdateField<TDocument>[] updateFields, UpdateOptions options = null) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("whereExpression is null.");
            }

            if (updateFields == null || !updateFields.Any())
            {
                throw new Exception("updateFields is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            var updateDefinitionBuilder = Builders<TDocument>.Update;
            var updateDefinitions =
                updateFields.Select(s => updateDefinitionBuilder.Set(s.Field, s.Value)).ToList();

            return await this._db.GetCollection<TDocument>(type.Name)
                .UpdateOneAsync(whereExpression, updateDefinitionBuilder.Combine(updateDefinitions), options);
        }

        /// <summary>
        /// 更新多个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="updateFields">更新字段和值</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public UpdateResult UpdateMany<TDocument>(Expression<Func<TDocument, bool>> whereExpression, UpdateField<TDocument>[] updateFields, UpdateOptions options = null) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("whereExpression is null.");
            }

            if (updateFields == null || !updateFields.Any())
            {
                throw new Exception("updateFields is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            var updateDefinitionBuilder = Builders<TDocument>.Update;
            var updateDefinitions =
                updateFields.Select(s => updateDefinitionBuilder.Set(s.Field, s.Value)).ToList();

            return this._db.GetCollection<TDocument>(type.Name)
                .UpdateMany(whereExpression, updateDefinitionBuilder.Combine(updateDefinitions), options);
        }

        /// <summary>
        /// 异步更新多个文档
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="updateFields">更新字段和值</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> whereExpression, UpdateField<TDocument>[] updateFields, UpdateOptions options = null) where TDocument : IMongoDbEntity
        {
            if (whereExpression == null)
            {
                throw new Exception("whereExpression is null.");
            }

            if (updateFields == null || !updateFields.Any())
            {
                throw new Exception("updateFields is null.");
            }

            var type = CheckHasBsonIdAttribute<TDocument>();

            var updateDefinitionBuilder = Builders<TDocument>.Update;
            var updateDefinitions =
                updateFields.Select(s => updateDefinitionBuilder.Set(s.Field, s.Value)).ToList();

            return await this._db.GetCollection<TDocument>(type.Name)
                .UpdateManyAsync(whereExpression, updateDefinitionBuilder.Combine(updateDefinitions), options);
        }

        #region 内部方法

        /// <summary>
        /// 判断BsonId是否有BsonIdAttribute
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Type CheckHasBsonIdAttribute<TDocument>()
        {
            var type = typeof(TDocument);
            var property = type.GetProperty(nameof(IMongoDbEntity.BsonId));
            var attr = Attribute.GetCustomAttribute(property, typeof(BsonIdAttribute));
            if (attr == null)
            {
                throw new Exception("BsonId missing BsonIdAttribute.");
            }

            return type;
        }

        #endregion
    }
}
