//using System.ComponentModel.DataAnnotations;

//namespace Biblioteca.Models
//{
//    public class PrestamoLibros
//    {
//        public int Id { get; set; }
//        public int LibroId { get; set; }
//        public string NombreCliente { get; set; }
//        public int Telefono { get; set; }
//        public string Correo { get; set; }

//        [DataType(DataType.Date, ErrorMessage = "La Fecha es obligatoria")]
//        public DateTime FechaPrestamo { get; set; }

//        [DataType(DataType.Date, ErrorMessage = "La Fecha es obligatoria")]
//        public DateTime FechaDevolucion { get; set; }
//    }
//}
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class PrestamoLibros
    {
        public int Id { get; set; }

        [Required]
        public int LibroId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del cliente debe tener entre 3 y 100 caracteres.")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(8, ErrorMessage = "El número de teléfono debe tener 8 dígitos.", MinimumLength = 8)]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El número de teléfono proporcionado no es válido.")]
        public string Telefono { get; set; }


        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico proporcionado no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La Fecha de Préstamo es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; }

        [Required(ErrorMessage = "La Fecha de Devolución es obligatoria")]
        [DataType(DataType.Date)]
        [DateGreaterThan("FechaPrestamo", ErrorMessage = "La fecha de devolución debe ser posterior a la fecha de préstamo.")]
        public DateTime FechaDevolucion { get; set; }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparedPropertyName;

        public DateGreaterThanAttribute(string comparedPropertyName)
        {
            _comparedPropertyName = comparedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var comparedProperty = validationContext.ObjectType.GetProperty(_comparedPropertyName);

            if (comparedProperty == null)
                throw new ArgumentException("La propiedad con nombre " + _comparedPropertyName + " no se encuentra");

            var comparedPropertyValue = (DateTime)comparedProperty.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparedPropertyValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
