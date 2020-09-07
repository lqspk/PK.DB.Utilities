using PK.DB.Utilities.Entities;
using PK.DB.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK.DB.Utilities.Test.Models {
    public class User : MongoDbEntity {

        public DateTime CreateTime { get; set; }
    }
}
