using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class HorasTrabajoController : Controller
    {
        private readonly HorasTrabajoRepositorio _horasTrabajoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public HorasTrabajoController(HorasTrabajoRepositorio horasTrabajoRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _horasTrabajoRepositorio = horasTrabajoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        // Acción para mostrar la lista de horas trabajadas
        public async Task<IActionResult> Index()
        {
            var horasTrabajadas = await _horasTrabajoRepositorio.GetAll();
            return View(horasTrabajadas);
        }

        // Acción para mostrar el formulario de creación
        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = await _empleadoRepositorio.GetAll(); // Para mostrar la lista de empleados
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorasTrabajo horasTrabajo)
        {
            if (ModelState.IsValid)
            {
                // Si el modelo es válido, guardamos en la base de datos
                await _horasTrabajoRepositorio.Add(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            // Si no es válido, recargamos el formulario con los empleados
            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            return View(horasTrabajo);
        }

    }
}
