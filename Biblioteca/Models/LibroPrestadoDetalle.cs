namespace Biblioteca.Models
{
    public class LibroPrestadoDetalle
    {
        public int PrestamoId { get; set; }  // Id del préstamo
        public string Titulo { get; set; }   // Título del libro
        public string NombreCliente { get; set; }
        public int Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
    }
}

