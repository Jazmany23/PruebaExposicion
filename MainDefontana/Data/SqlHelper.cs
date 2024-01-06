using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace MainDefontana.Data
{
    public class SqlHelper
    {
        private readonly string _conexion = "";

        public SqlHelper() { 
            _conexion = @"Data Source=lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com;Initial Catalog=Prueba; user id=ReadOnly; Password=d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU;";
        }

        public async Task<List<T>> SimpleQueryAsync<T>(string query, object? parametros = null)
        {
            using var connection = new SqlConnection(_conexion);
            var results = connection.Query<T>(query);
            return results.ToList();
           
        }
    }
}
