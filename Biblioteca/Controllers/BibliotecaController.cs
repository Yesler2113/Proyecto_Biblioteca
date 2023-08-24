using Biblioteca.Models;
using Biblioteca.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class BibliotecaController : Controller
    {
        private readonly ILogger<BibliotecaController> logger;
        private readonly IRepositorioBiblioteca repositorioBiblioteca;
        private readonly IServicioLibros servicioLibros;

        public BibliotecaController(ILogger<BibliotecaController> logger,
            IRepositorioBiblioteca repositorioBiblioteca, IServicioLibros servicioLibros)
        {
            this.logger = logger;
            this.repositorioBiblioteca = repositorioBiblioteca;
            this.servicioLibros = servicioLibros;
        }

        //public IActionResult NoEncontrado()
        //{
        //    return View();
        //}


        public async Task<IActionResult> Index()
        {
            var libros = await repositorioBiblioteca.ObtenerTodos();
            return View(libros);
        }




        public IConfiguration Configuration { get; }



        [HttpGet]

        public IActionResult Guardar()
        {
              return View();
        }

        [HttpPost]

        public async Task<IActionResult> Guardar(Libros modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            modelo.Id = servicioLibros.ObtenerLibrosId();

            var existe = await repositorioBiblioteca.Existe(modelo.Titulo, modelo.Id);

            if (existe)
            {
                ModelState.AddModelError(nameof(modelo.Titulo), $"El libro {modelo.Titulo} ya existe");
                return View(modelo);
            }

            await repositorioBiblioteca.Guardar(modelo);

            return RedirectToAction("Index");


        }

        [HttpGet]
        public async Task<IActionResult> Actualizar(int id)
        {
            var libros = await repositorioBiblioteca.ObtenerLibroPorId(id);

            if (libros == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(libros);
        }

        [HttpPost]
        
        public async Task<IActionResult> Actualizar(Libros modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            await repositorioBiblioteca.Actualizar(modelo);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> VerificarExisteLibro(string titulo)
        {
            var tituloId = servicioLibros.ObtenerLibrosId();
            var existe = await repositorioBiblioteca.Existe(titulo, tituloId);

            if (existe)
            {
                return Json($"El libro {titulo} ya existe");
            }

            return Json(true);
        }

        [HttpGet]

        public async Task<IActionResult> Eliminar(int id)
        {
            var librosId = servicioLibros.ObtenerLibrosId();
            //var libros = await repositorioBiblioteca.ObtenerLibroId(id, librosId);
            var libros = await repositorioBiblioteca.ObtenerLibroPorId(id);
            if (libros == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(libros);
        }

        [HttpPost]

        public async Task<IActionResult> EliminarLibro(int id)
        {
            var librosId = servicioLibros.ObtenerLibrosId();
            //var libros = await repositorioBiblioteca.ObtenerLibroId(id, librosId);
            var libros = await repositorioBiblioteca.ObtenerLibroPorId(id);

            if (libros is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioBiblioteca.Eliminar(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                return RedirectToAction("Index");
            }

            var librosEncontrados = await repositorioBiblioteca.BuscarPorTitulo(titulo);
            return View("Index", librosEncontrados);
        }



    }
}
