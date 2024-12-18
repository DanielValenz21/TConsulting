﻿using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class PrestamoRepositorio
    {
        private readonly string _connectionString;

        public PrestamoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabaseConnection");
        }

        // Obtener todos los préstamos
        public async Task<IEnumerable<Prestamo>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Prestamos";
                return await connection.QueryAsync<Prestamo>(query);
            }
        }
        public async Task<IEnumerable<Prestamo>> ObtenerPrestamosActivosPorEmpleado(int idEmpleado)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT * FROM Prestamos 
                      WHERE IdEmpleado = @IdEmpleado AND CuotasPendientes > 0";
                return await connection.QueryAsync<Prestamo>(query, new { IdEmpleado = idEmpleado });
            }
        }

        // Obtener un préstamo por ID
        public async Task<Prestamo> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Prestamos WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Prestamo>(query, new { Id = id });
            }
        }

        // Insertar un nuevo préstamo
        public async Task<int> Add(Prestamo prestamo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Prestamos (IdEmpleado, Total, CuotasPendientes, FechaPrestamo, IdTipo) 
                              VALUES (@IdEmpleado, @Total, @CuotasPendientes, @FechaPrestamo, @IdTipo)";
                return await connection.ExecuteAsync(query, prestamo);
            }
        }

        // Actualizar un préstamo
        public async Task<int> Update(Prestamo prestamo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Prestamos SET 
                              IdEmpleado = @IdEmpleado, 
                              Total = @Total, 
                              CuotasPendientes = @CuotasPendientes, 
                              FechaPrestamo = @FechaPrestamo, 
                              IdTipo = @IdTipo
                              WHERE Id = @Id";
                return await connection.ExecuteAsync(query, prestamo);
            }
        }

        // Eliminar un préstamo
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Prestamos WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
