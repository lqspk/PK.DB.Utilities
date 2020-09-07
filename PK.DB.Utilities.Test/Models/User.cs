using PK.DB.Utilities.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK.DB.Utilities.Test.Models {
    public class User : MongoDbEntity {
        public override string BsonId { get; set; }

        public DateTime CreateTime { get; set; }
        
    }
}
