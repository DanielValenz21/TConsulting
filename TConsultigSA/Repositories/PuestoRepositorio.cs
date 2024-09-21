using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class PuestoRepositorio
    {
        private readonly string _connectionString;

        public PuestoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Puesto>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var query = "SELECT * FROM Puestos";
                    var result = await connection.QueryAsync<Puesto>(query);
                    if (result == null)
                    {
                        throw new System.Exception("No se encontraron puestos.");
                    }
                    return result;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("Error al ejecutar la consulta de puestos: " + ex.Message);
                }
            }
        }
    }
}
