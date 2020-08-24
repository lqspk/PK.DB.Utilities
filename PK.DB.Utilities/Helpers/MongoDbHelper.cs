using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PK.DB.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PK.DB.Utilities.Helpers
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
        /// 新增一个文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns> 
        public void Insert<T>(T entity) where T: IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }

            this._db.GetCollection<T>(entity.GetType().Name).InsertOne(entity);
        }

        /// <summary>
        /// 异步新增一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync<T>(T entity) where T : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }

            await this._db.GetCollection<T>(entity.GetType().Name).InsertOneAsync(entity);
        }

        /// <summary>
        /// 批量新增文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void InsertMany<T>(IList<T> entities) where T : IMongoDbEntity
        {
            if (entities != null)
            {
                var entity = entities.FirstOrDefault();
                if (entity != null)
                {
                    this._db.GetCollection<T>(entity.GetType().Name).InsertMany(entities);
                }
            }
        }

        /// <summary>
        /// 异步批量新增文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public async Task InsertManyAsync<T>(IList<T> entities) where T : IMongoDbEntity
        {
            if (entities != null)
            {
                var entity = entities.FirstOrDefault();
                if (entity != null)
                {
                    await this._db.GetCollection<T>(entity.GetType().Name).InsertManyAsync(entities);
                }
            }
        }

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public ReplaceOneResult ReplaceOne<T>(T entity, ReplaceOptions options = null) where T : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }

            return this._db.GetCollection<T>(entity.GetType().Name)
                .ReplaceOne(s => s.Id == entity.Id, entity, options);
        }

        /// <summary>
        /// 异步替换单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> ReplaceOneAsync<T>(T entity, ReplaceOptions options = null) where T : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }
            
            return await this._db.GetCollection<T>(entity.GetType().Name)
                .ReplaceOneAsync(s => s.Id == entity.Id, entity, options);
        }

        /// <summary>
        /// 删除单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DeleteResult DeleteOne<T>(T entity) where T : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }

            return this._db.GetCollection<T>(entity.GetType().Name)
                .DeleteOne(s => s.Id == entity.Id);
        }

        /// <summary>
        /// 异步删除单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteOneAsync<T>(T entity) where T : IMongoDbEntity
        {
            if (entity == null)
            {
                throw new Exception("entity is null.");
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new Exception("Id is null or empty.");
            }

            return await this._db.GetCollection<T>(entity.GetType().Name)
                .DeleteOneAsync(s => s.Id == entity.Id);
        }
        
        /// <summary>
        /// 删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DeleteResult DeleteMany<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            if (expression == null)
            {
                throw new Exception("expression is null.");
            }

            return this._db.GetCollection<T>(typeof(T).Name)
                .DeleteMany(expression);
        }

        /// <summary>
        /// 异步删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            if (expression == null)
            {
                throw new Exception("expression is null.");
            }

            return await this._db.GetCollection<T>(typeof(T).Name)
                .DeleteManyAsync(expression);
        }

        /// <summary>
        /// 查找单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T FindOne<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            return IAsyncCursorSourceExtensions.FirstOrDefault(this._db.GetCollection<T>(typeof(T).Name)
                    .AsQueryable()
                    .Where<T>(expression));
        }

        /// <summary>
        /// 异步查找单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> FindOneAsync<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            return await this._db.GetCollection<T>(typeof(T).Name)
                .AsQueryable()
                .Where<T>(expression)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查找文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IMongoQueryable<T> Find<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            return this._db.GetCollection<T>(typeof(T).Name)
                .AsQueryable()
                .Where<T>(expression);
        }

        /// <summary>
        /// 查找文档列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<T> FindList<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            return this._db.GetCollection<T>(typeof(T).Name)
                .AsQueryable()
                .Where<T>(expression)
                .ToList();
        }

        /// <summary>
        /// 异步查找文档列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IList<T>> FindListAsync<T>(Expression<Func<T, bool>> expression) where T : IMongoDbEntity
        {
            return await this._db.GetCollection<T>(typeof(T).Name)
                .AsQueryable()
                .Where<T>(expression)
                .ToListAsync();
        }
    }
}
