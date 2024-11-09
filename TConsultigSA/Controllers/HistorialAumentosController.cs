using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class HistorialAumentosController : Controller
    {
        private readonly HistorialAumentoRepositorio _historialAumentoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public HistorialAumentosController(HistorialAumentoRepositorio historialAumentoRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _historialAumentoRepositorio = historialAumentoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        public async Task<IActionResult> Index(int? idEmpleado)
        {
            if (!idEmpleado.HasValue)
            {
                var empleados = await _empleadoRepositorio.GetAll();
                return View("SeleccionarEmpleado", empleados);
            }

            var empleado = await _empleadoRepositorio.GetById(idEmpleado.Value);
            if (empleado == null) return NotFound();

            ViewBag.Empleado = empleado;
            var historial = await _historialAumentoRepositorio.GetByEmpleadoId(idEmpleado.Value);
            return View(historial);
        }

        public async Task<IActionResult> Create(int idEmpleado)
        {
            var empleado = await _empleadoRepositorio.GetById(idEmpleado);
            if (empleado == null) return NotFound();

            ViewBag.Empleado = empleado;
            return View(new HistorialAumento { IdEmpleado = idEmpleado, Fecha = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistorialAumento historialAumento)
        {
            if (ModelState.IsValid)
            {
                await _historialAumentoRepositorio.Add(historialAumento);
                return RedirectToAction(nameof(Index), new { idEmpleado = historialAumento.IdEmpleado });
            }

            var empleado = await _empleadoRepositorio.GetById(historialAumento.IdEmpleado);
            ViewBag.Empleado = empleado;
            return View(historialAumento);
        }
    }
}
