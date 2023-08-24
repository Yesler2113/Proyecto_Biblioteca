using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Models
{
    public class PrestarLibroViewModel : PrestamoLibros
    {
        public IEnumerable<SelectListItem> Libros { get; set; }
    }
}
