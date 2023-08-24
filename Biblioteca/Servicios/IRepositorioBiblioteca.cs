using Biblioteca.Models;

namespace Biblioteca.Servicios
{
    public interface IRepositorioBiblioteca
    {
        Task Actualizar(Libros modelo);
        Task<IEnumerable<Libros>> BuscarPorTitulo(string titulo);
        Task DisminuirCantidadLibro(int libroId);
        Task Eliminar(int id);
        Task<bool> Existe(string titulo, int id);
        Task Guardar(Libros modelo);
        Task IncrementarCantidadLibro(int libroId);
        Task<IEnumerable<Libros>> Obtener(int id);
        Task<Libros> ObtenerLibroId(int id, int libroId);
        Task<Libros> ObtenerLibroPorId(int id);
        Task<IEnumerable<Libros>> ObtenerTodos();
    }
}