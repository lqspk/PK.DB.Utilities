using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PK.DB.Utilities.MongoDb;
using PK.DB.Utilities.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace PK.DB.Utilities.Test
{
    /// <summary>
    /// MongoDb测试单元
    /// </summary>
    [TestClass]
    public class UnitTestMongoDB
    {
        private static MongoDbHelper dbHelper = new MongoDbHelper(new MongoUrl("mongodb://localhost:27017/Demo"));


        [TestMethod]
        public void TestInsert()
        {
            try
            {
                User user = new User()
                {
                    BsonId = Guid.NewGuid().ToString("N"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };

                dbHelper.Insert(user);

                Console.WriteLine("true");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        delegate object updateAction (Func<User, Object> field, object value);

        [TestMethod]
        public void TestUpdateOne()
        {
            

            try
            {
                User user = dbHelper.FindOne<User>(s => s.BsonId == "650805e8f9704abf955006da63eaa12b");
                var result = dbHelper.UpdateOne<User>(s => s.BsonId == user.BsonId, new []
                {
                    new UpdateField<User>(){ Field = s => s.CreateTime, Value = DateTime.UtcNow },
                    new UpdateField<User>(){ Field = s => s.Name, Value = "李四" },
                });

                Console.WriteLine("true");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestUpdateMany()
        {
            try
            {
                var result = dbHelper.UpdateMany<User>(s => !s.Status.HasValue, new[]
                {
                    new UpdateField<User>(){ Field = s => s.Status, Value = 1 }
                });

                Console.WriteLine("true");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestReplaceOne()
        {
            try
            {
                User user = dbHelper.FindOne<User>(s => s.BsonId == "e16a022f658c4037a3cc5cfbc7c296d5");
                user.CreateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                dbHelper.ReplaceOne(user);

                Console.WriteLine("true");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestFind()
        {
            try
            {
                var query = dbHelper.Find<User>(s => s.BsonId == "e16a022f658c4037a3cc5cfbc7c296d5");

                query = query.Where(s => s.CreateTime < DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

                var list = query.ToList();

                Console.WriteLine(list.Count.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        [TestMethod]
        public void TestDeleteMany()
        {
            try
            {
                var result = dbHelper.DeleteMany<User>(s =>
                    new string[] {"e16a022f658c4037a3cc5cfbc7c296d5"}.Contains(s.BsonId));

                Console.WriteLine(result.DeletedCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
