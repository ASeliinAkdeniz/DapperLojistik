using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLojistik.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}