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

        // Mostrar lista de horas trabajadas
        public async Task<IActionResult> Index()
        {
            var horasTrabajadas = await _horasTrabajoRepositorio.GetAll();
            return View(horasTrabajadas);
        }

        // Mostrar formulario de creación
        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = await _empleadoRepositorio.GetAll(); // Obtener lista de empleados
            return View();
        }

        // Procesar formulario de creación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorasTrabajo horasTrabajo)
        {
            if (ModelState.IsValid)
            {
                await _horasTrabajoRepositorio.Add(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            return View(horasTrabajo);
        }
    }
}
