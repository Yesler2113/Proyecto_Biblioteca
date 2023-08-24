using Biblioteca.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Biblioteca.Servicios
{

    public class RepositorioPrestarLibro : IRepositorioPrestarLibro
    {
        private string connectionString;
        private readonly ILogger<RepositorioPrestarLibro> logger;

        public RepositorioPrestarLibro(IConfiguration configuration, ILogger<RepositorioPrestarLibro> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }


      
        public async Task PrestarLibro(PrestamoLibros modelo)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"
                INSERT INTO LibrosPrestados(LibroId, NombreCliente, Telefono, Correo, FechaPrestamo, FechaDevolucion)
                VALUES (@LibroId, @NombreCliente, @Telefono, @Correo, @FechaPrestamo, @FechaDevolucion)
                SELECT SCOPE_IDENTITY()", modelo);

            modelo.Id = id;
        }

        public async Task<IEnumerable<LibroPrestadoDetalle>> ObtenerDetalleLibrosPrestados()
        {
            using var connection = new SqlConnection(connectionString);
            var query = @"
                        SELECT 
                            p.Id as PrestamoId,
                            l.Titulo,
                            p.NombreCliente,
                            p.Telefono,
                            p.Correo,
                            p.FechaPrestamo,
                            p.FechaDevolucion
                        FROM LibrosPrestados p
                        INNER JOIN LibrosBiblioteca l ON p.LibroId = l.Id";

            return await connection.QueryAsync<LibroPrestadoDetalle>(query);
        }

        public async Task Eliminar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM LibrosPrestados WHERE Id = @id;", new { Id = id });
            logger.LogInformation($"Se elimino el libro con el id: {id}");
        }

        public async Task<PrestamoLibros> ObtenerLibroPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            var query = "SELECT * FROM LibrosPrestados WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<PrestamoLibros>(query, new { Id = id });
        }


        public async Task<IEnumerable<PrestamoLibros>> ObtenerLibrosPrestados()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<PrestamoLibros>("SELECT * FROM LibrosPrestados");
        }

    }
}
