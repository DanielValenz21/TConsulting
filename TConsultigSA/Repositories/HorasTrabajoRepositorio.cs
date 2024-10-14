using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultingSA.Repositories;

namespace TConsultigSA.Repositories
{
    public class HorasTrabajoRepositorio : IHorasTrabajoRepositorio
    {
        private readonly string _connectionString;

        public HorasTrabajoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Implementación del método para obtener todas las horas trabajadas
        public async Task<IEnumerable<HorasTrabajo>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ht.*, e.*
                              FROM HorasTrabajo ht
                              INNER JOIN Empleados e ON ht.IdEmpleado = e.Id";

                var horasTrabajadas = await connection.QueryAsync<HorasTrabajo, Empleado, HorasTrabajo>(
                    query,
                    (horasTrabajo, empleado) =>
                    {
                        horasTrabajo.Empleado = empleado;
                        return horasTrabajo;
                    },
                    splitOn: "IdEmpleado"
                );

                return horasTrabajadas;
            }
        }

        public async Task<int> Add(HorasTrabajo horasTrabajo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO HorasTrabajo (IdEmpleado, Fecha, TotalHoras, Observaciones, Aprobado)
                      VALUES (@IdEmpleado, @Fecha, @TotalHoras, @Observaciones, @Aprobado)";
                return await connection.ExecuteAsync(query, horasTrabajo);
            }
        }

    }
}
