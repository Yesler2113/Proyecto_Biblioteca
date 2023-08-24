using Biblioteca.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Servicios
{
    public class RepositorioBiblioteca : IRepositorioBiblioteca
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RepositorioBiblioteca> logger;
        private readonly string connectionString;

        public RepositorioBiblioteca(IConfiguration configuration, ILogger<RepositorioBiblioteca> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task<Libros> ObtenerLibroId(int id, int cantidad)
        {
            using var conecction = new SqlConnection(connectionString);
            return await conecction.QueryFirstOrDefaultAsync<Libros>
                (@"
                SELECT 
                    Id,
                    Titulo,
                    Autor,
                    Genero,
                    Cantidad
                FROM LibrosBiblioteca
                WHERE Id = @Id And Cantidad = @cantidad", new { id, cantidad });

        }

        //cambios
        public async Task<Libros> ObtenerLibroPorId(int id)
        {
            using var conecction = new SqlConnection(connectionString);
            return await conecction.QueryFirstOrDefaultAsync<Libros>
                (@"
            SELECT 
                Id,
                Titulo,
                Autor,
                Genero,
                Cantidad
            FROM LibrosBiblioteca
            WHERE Id = @Id", new { id });
        }


        public async Task Guardar(Libros modelo)
        {
            using var conecction = new SqlConnection(connectionString);
            var id = await conecction.QuerySingleAsync<int>
                (@"
                INSERT INTO LibrosBiblioteca (Titulo, Autor, Genero, Cantidad)
                VALUES (@Titulo, @Autor, @Genero, @Cantidad)
                SELECT SCOPE_IDENTITY()", modelo);

            modelo.Id = id;
            logger.LogInformation($"Se creo el libro con el nombre: {modelo.Titulo}");
        }

        //cambios

        public async Task<IEnumerable<Libros>> ObtenerTodos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Libros>("SELECT Id, Titulo, Autor, Genero, Cantidad FROM LibrosBiblioteca");
        }


        public async Task Actualizar(Libros modelo)
        {
            using var conecction = new SqlConnection(connectionString);
            await conecction.ExecuteAsync
                (@"
                UPDATE LibrosBiblioteca
                SET Titulo = @Titulo,
                    Autor = @Autor,
                    Genero = @Genero,
                    Cantidad = @Cantidad
                WHERE Id = @Id", modelo);

            logger.LogInformation($"Se actualizo el libro con el nombre: {modelo.Titulo}");
        }

        public async Task Eliminar(int id)
        {
            using var conecction = new SqlConnection(connectionString);
            await conecction.ExecuteAsync
                (@"
                    DELETE FROM LibrosBiblioteca WHERE Id = @id;", new { id });

            logger.LogInformation($"Se elimino el libro con el id: {id}");
        }

        public async Task<IEnumerable<Libros>> Obtener(int id)
        {
            using var conecction = new SqlConnection(connectionString);
            return await conecction.QueryAsync<Libros>
                (@"
                SELECT 
                    Id,
                    Titulo,
                    Autor,
                    Genero,
                    Cantidad
                FROM LibrosBiblioteca
                WHERE Id = @id", new {id});

        }


        public async Task<bool> Existe(string titulo, int id)
        {
            using var conecction = new SqlConnection(connectionString);
            var existe = await conecction.ExecuteScalarAsync<bool>
                (@"
                SELECT 
                    COUNT(1)
                FROM LibrosBiblioteca
                WHERE Titulo = @Titulo AND Id <> @Id", new { titulo, id });
            return existe;
        }

        public async Task DisminuirCantidadLibro(int libroId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "UPDATE LibrosBiblioteca SET Cantidad = Cantidad - 1 WHERE Id = @LibroId AND Cantidad > 0",
                new { LibroId = libroId });
        }

        public async Task IncrementarCantidadLibro(int libroId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE LibrosBiblioteca SET Cantidad = Cantidad + 1 WHERE Id = @Id", new { Id = libroId });
        }

        public async Task<IEnumerable<Libros>> BuscarPorTitulo(string titulo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Libros>(
                "SELECT Id, Titulo, Autor, Genero, Cantidad FROM LibrosBiblioteca WHERE Titulo LIKE @Titulo",
                new { Titulo = $"%{titulo}%" });
        }





    }
}
