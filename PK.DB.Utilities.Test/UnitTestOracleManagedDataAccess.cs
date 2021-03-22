using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PK.DB.Utilities.Oracle.Helpers;

namespace PK.DB.Utilities.Test
{
    [TestClass]
    public class UnitTestOracleManagedDataAccess
    {
        private readonly OracleManagedDataAccessHelper helper = new OracleManagedDataAccessHelper("Data Source=oracle;User Id=oracleuser;Password=pwd");
        [TestMethod]
        public void TestExecuteQuerySql()
        {
            var dt = helper.ExecuteQuerySql("select * from v$version");
        }
    }
}
