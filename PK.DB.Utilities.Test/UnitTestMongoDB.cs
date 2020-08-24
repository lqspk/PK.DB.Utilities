using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PK.DB.Utilities.Helpers;
using PK.DB.Utilities.Interfaces;
using System;
using System.Linq;

namespace PK.DB.Utilities.Test
{
    /// <summary>
    /// MongoDb测试单元
    /// </summary>
    [TestClass]
    public class UnitTestMongoDB
    {
        private static MongoDbHelper dbHelper = new MongoDbHelper("mongodb://localhost:27017/", "Demo");

        [TestMethod]
        public void TestInsert()
        {
            try
            {
                User user = new User()
                {
                    Id = Guid.NewGuid().ToString("N"),
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

        [TestMethod]
        public void TestReplaceOne()
        {
            try
            {
                User user = dbHelper.FindOne<User>(s => s.Id == "e16a022f658c4037a3cc5cfbc7c296d5");
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
                var query = dbHelper.Find<User>(s => s.Id == "e16a022f658c4037a3cc5cfbc7c296d5");

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
                    new string[] {"e16a022f658c4037a3cc5cfbc7c296d5"}.Contains(s.Id));

                Console.WriteLine(result.DeletedCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }

    public class User : IMongoDbEntity
    {
        public string Id { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
