using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class HorasTrabajoRepositorio
    {
        private readonly string _connectionString;

        public HorasTrabajoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Insertar las horas de trabajo en la base de datos
        public async Task<int> Add(HorasTrabajo horasTrabajo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO HorasTrabajo (IdEmpleado, Fecha, HoraEntrada, HoraSalida, TotalHoras, Observaciones) 
                      VALUES (@IdEmpleado, @Fecha, @HoraEntrada, @HoraSalida, @TotalHoras, @Observaciones)";
                return await connection.ExecuteAsync(query, horasTrabajo);
            }
        }

        // Obtener todas las horas trabajadas
        public async Task<IEnumerable<HorasTrabajo>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM HorasTrabajo";
                return await connection.QueryAsync<HorasTrabajo>(query);
            }
        }

        // Obtener las horas trabajadas por un empleado específico
        public async Task<HorasTrabajo> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM HorasTrabajo WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<HorasTrabajo>(query, new { Id = id });
            }
        }
    }
}
