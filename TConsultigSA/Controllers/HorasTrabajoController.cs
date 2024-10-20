using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using TConsultingSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class HorasTrabajoController : Controller
    {
        private readonly IHorasTrabajoRepositorio _horasRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public HorasTrabajoController(IHorasTrabajoRepositorio horasRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _horasRepositorio = horasRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var horas = await _horasRepositorio.GetAll();
            return View(horas);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorasTrabajo horasTrabajo)
        {
            if (ModelState.IsValid)
            {
                horasTrabajo.IdEmpleado = horasTrabajo.IdEmpleado;
                await _horasRepositorio.Add(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View(horasTrabajo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            // En lugar de un select, mostramos el nombre del empleado y lo mantenemos en ViewBag
            ViewBag.EmpleadoNombre = horasTrabajo.Empleado?.Nombre;

            return View(horasTrabajo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HorasTrabajo horasTrabajo)
        {
            if (ModelState.IsValid)
            {
                await _horasRepositorio.Update(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }
            return View(horasTrabajo);
        }
        public async Task<IActionResult> Details(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            // Calculo de horas extras (más de 160 horas al mes)
            const int horasSemanales = 40;
            const int semanasPorMes = 4;
            var horasNormales = horasSemanales * semanasPorMes; // 160 horas
            var horasExtras = horasTrabajo.TotalHoras > horasNormales ? horasTrabajo.TotalHoras - horasNormales : 0;

            // Crear un modelo de vista para enviar la información necesaria al modal
            var viewModel = new DetallesHorasViewModel
            {
                NombreEmpleado = horasTrabajo.Empleado?.Nombre,
                Mes = horasTrabajo.Fecha.ToString("MM/yyyy"),
                HorasTrabajadas = horasTrabajo.TotalHoras,
                HorasExtras = horasExtras,
                Observaciones = horasTrabajo.Observaciones,
                Estado = horasTrabajo.Aprobado ? "Aprobado" : "Pendiente"
            };

            return PartialView("_DetallesHorasModal", viewModel); // Devolveremos un PartialView
        }


        // DELETE methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            await _horasRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
