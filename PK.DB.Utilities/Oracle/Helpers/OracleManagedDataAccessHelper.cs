using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace PK.DB.Utilities.Oracle.Helpers
{
    public class OracleManagedDataAccessHelper : IDisposable
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        private readonly OracleConnection _connection = null;

        public OracleConnection Connection
        {
            get { return _connection; }
        }

        public OracleManagedDataAccessHelper(string connectionString)
        {
            _connection = new OracleConnection(connectionString);
        }

        public OracleManagedDataAccessHelper(string connectionString, OracleCredential orclCredential)
        {
            _connection = new OracleConnection(connectionString, orclCredential);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        public DataTable ExecuteQuerySql(string sql)
        {
            var da = new OracleDataAdapter(sql, _connection);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
