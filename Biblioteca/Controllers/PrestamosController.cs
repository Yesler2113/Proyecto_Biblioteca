using Biblioteca.Models;
using Biblioteca.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Abstractions;

namespace Biblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly IRepositorioPrestarLibro repositorioPrestarLibro;
        private readonly IRepositorioBiblioteca repositorioBiblioteca;
        private readonly IServicioLibros servicioLibros;

        public PrestamosController(
            IRepositorioPrestarLibro repositorioPrestarLibro,
            IRepositorioBiblioteca repositorioBiblioteca,
            IServicioLibros servicioLibros)
        {
            this.repositorioPrestarLibro = repositorioPrestarLibro;
            this.repositorioBiblioteca = repositorioBiblioteca;
            this.servicioLibros = servicioLibros;
        }
        public async Task<IActionResult> Index()
        {
            var librosPrestados = await repositorioPrestarLibro.ObtenerDetalleLibrosPrestados();
            return View(librosPrestados);
        }


        [HttpGet]

        public async Task<IActionResult> PrestarLibro()
        {
            var usuarioId = servicioLibros.ObtenerLibrosId();

            var modelo = new PrestarLibroViewModel();

            modelo.Libros = await ObtenerLibros();
            return View(modelo);

        }

        //[HttpPost]
        //public async Task<IActionResult> PrestarLibro(PrestarLibroViewModel modelo)
        //{
        //    var libroId = servicioLibros.ObtenerLibrosId();

        //    var prestamos = await repositorioBiblioteca.ObtenerLibroPorId(modelo.LibroId);

        //    if (prestamos == null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        modelo.Libros = await ObtenerLibros();
        //        return View (modelo);
        //    }

        //    await repositorioPrestarLibro.PrestarLibro(modelo);
        //    await repositorioBiblioteca.DisminuirCantidadLibro(modelo.LibroId);

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> PrestarLibro(PrestarLibroViewModel modelo)
        {
            var libroId = servicioLibros.ObtenerLibrosId();

            var prestamos = await repositorioBiblioteca.ObtenerLibroPorId(modelo.LibroId);

            if (prestamos == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            // Verificación de disponibilidad del libro
            if (prestamos.Cantidad <= 0)
            {
                ModelState.AddModelError("", "No hay ejemplares disponibles de este libro.");
                modelo.Libros = await ObtenerLibros();
                return View(modelo);
            }

            if (!ModelState.IsValid)
            {
                modelo.Libros = await ObtenerLibros();
                return View(modelo);
            }

            await repositorioPrestarLibro.PrestarLibro(modelo);
            await repositorioBiblioteca.DisminuirCantidadLibro(modelo.LibroId);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> VerLibrosPrestados()
        {
            var librosPrestados = await repositorioPrestarLibro.ObtenerDetalleLibrosPrestados();
            return View(librosPrestados);
        }

        //[HttpGet]
        //public async Task<IActionResult> Eliminar(int libroId)
        //{
        //    var libro = await repositorioPrestarLibro.ObtenerLibroPorId(libroId);
        //    if (libro == null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }
        //    return View(libro);
        //}


        ////[HttpPost, ActionName("Eliminar")]
        ////[ValidateAntiForgeryToken] // Esta anotación añade protección contra ataques CSRF
        ////public async Task<IActionResult> ConfirmarEliminar(int libroId)
        ////{
        ////    await repositorioPrestarLibro.EliminarLibroPrestado(libroId);
        ////    return RedirectToAction("Index");
        ////}



        //[HttpPost]
        //public async Task<IActionResult> EliminarLibroPrestado(int id)
        //{
        //    await repositorioPrestarLibro.EliminarLibroPrestado(id);
        //    return RedirectToAction("VerLibrosPrestados");
        //}


        [HttpGet]

        public async Task<IActionResult> Eliminar(int id)
        {
            var libro = await repositorioPrestarLibro.ObtenerLibroPorId(id);
            if (libro == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(libro);
        }

        [HttpPost]

        public async Task<IActionResult> EliminarLibroPrestado(int id)
        {
            var libros = await repositorioPrestarLibro.ObtenerLibroPorId(id);
            if (libros == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioPrestarLibro.Eliminar(id);

            await repositorioBiblioteca.IncrementarCantidadLibro(libros.LibroId);

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerLibros()
        {
            var listaLibros = await repositorioBiblioteca.ObtenerTodos();
            return listaLibros
                .Select(x => new SelectListItem(x.Titulo, x.Id.ToString()))
                .Prepend(new SelectListItem("Seleccione un Libro", ""));
        }
    }
}
