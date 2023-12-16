using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperWebApi.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("VillaDbConn");

        }

        public IDbConnection GetDbConnection() => new SqlConnection(_connectionString);
    }
}
