using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class DepartamentoRepositorio
    {
        private readonly string _connectionString;

        public DepartamentoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Departamento>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var query = "SELECT * FROM Departamentos";
                    var result = await connection.QueryAsync<Departamento>(query);
                    if (result == null)
                    {
                        throw new System.Exception("No se encontraron departamentos.");
                    }
                    return result;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("Error al ejecutar la consulta de departamentos: " + ex.Message);
                }
            }
        }
    }
}
