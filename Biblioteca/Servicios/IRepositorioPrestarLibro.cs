using Biblioteca.Models;

namespace Biblioteca.Servicios
{
    public interface IRepositorioPrestarLibro
    {
        Task Eliminar(int id);
        Task<IEnumerable<LibroPrestadoDetalle>> ObtenerDetalleLibrosPrestados();
        Task<PrestamoLibros> ObtenerLibroPorId(int id);
        Task PrestarLibro(PrestamoLibros modelo);
    }
}