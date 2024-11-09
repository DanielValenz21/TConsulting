using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class HistorialAumentoRepositorio
    {
        private readonly string _connectionString;

        public HistorialAumentoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabaseConnection");
        }

        public async Task<IEnumerable<HistorialAumento>> GetByEmpleadoId(int idEmpleado)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM HistorialAumentos WHERE IdEmpleado = @IdEmpleado";
            return await connection.QueryAsync<HistorialAumento>(query, new { IdEmpleado = idEmpleado });
        }

        public async Task Add(HistorialAumento historialAumento)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO HistorialAumentos (IdEmpleado, Fecha, PorcentajeAumento, CantidadAumento, SalarioFinal) 
                          VALUES (@IdEmpleado, @Fecha, @PorcentajeAumento, @CantidadAumento, @SalarioFinal)";
            await connection.ExecuteAsync(query, historialAumento);
        }
    }
}
