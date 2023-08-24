using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libros
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Titulo es obligatorio")]
        [Remote(action: "VerificarExisteLibro", controller: "Biblioteca")]

        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La cantidad de letras debe ser entre 3 y 50")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El Autor es obligatorio")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La cantidad de letras debe ser entre 3 y 50")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El Genero es obligatorio")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La cantidad de letras debe ser entre 3 y 50")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatorio")]
        public int Cantidad { get; set; }
    }
}
