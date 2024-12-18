﻿using Dapper;
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
            _connectionString = configuration.GetConnectionString("MyDatabaseConnection");
        }
        public async Task<IEnumerable<HorasTrabajo>> ObtenerHorasPorEmpleadoYMes(int idEmpleado, int mes, int año)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT * FROM HorasTrabajo 
                      WHERE IdEmpleado = @IdEmpleado AND MONTH(Fecha) = @Mes AND YEAR(Fecha) = @Año";
                return await connection.QueryAsync<HorasTrabajo>(query, new { IdEmpleado = idEmpleado, Mes = mes, Año = año });
            }
        }

        public async Task<IEnumerable<HorasTrabajo>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ht.Id, ht.IdEmpleado, ht.Fecha, ht.TotalHoras, ht.Observaciones, ht.Aprobado, 
                             e.Id, e.Nombre, e.DPI, e.Email
                      FROM HorasTrabajo ht
                      INNER JOIN Empleados e ON ht.IdEmpleado = e.Id";

                var horasTrabajadas = await connection.QueryAsync<HorasTrabajo, Empleado, HorasTrabajo>(
                    query,
                    (horasTrabajo, empleado) =>
                    {
                        horasTrabajo.Empleado = empleado;
                        return horasTrabajo;
                    },
                    splitOn: "Id"
                );

                return horasTrabajadas;
            }
        }
        public async Task<HorasTrabajo> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ht.*, e.* 
                      FROM HorasTrabajo ht
                      INNER JOIN Empleados e ON ht.IdEmpleado = e.Id
                      WHERE ht.Id = @Id";

                var horasTrabajadas = await connection.QueryAsync<HorasTrabajo, Empleado, HorasTrabajo>(
                    query,
                    (horasTrabajo, empleado) =>
                    {
                        horasTrabajo.Empleado = empleado;
                        return horasTrabajo;
                    },
                    new { Id = id }, // Pasamos el parámetro a la consulta
                    splitOn: "IdEmpleado"
                );

                return horasTrabajadas.FirstOrDefault(); // Retornamos el primer registro (si existe)
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
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM HorasTrabajo WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
        public async Task<int> Update(HorasTrabajo horasTrabajo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            UPDATE HorasTrabajo 
            SET 
                IdEmpleado = @IdEmpleado,
                Fecha = @Fecha,
                TotalHoras = @TotalHoras,
                Observaciones = @Observaciones,
                Aprobado = @Aprobado
            WHERE Id = @Id";

                return await connection.ExecuteAsync(query, horasTrabajo);
            }
        }


    }
}
