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
                // Asignamos el IdEmpleado directamente al modelo antes de insertarlo
                horasTrabajo.IdEmpleado = horasTrabajo.IdEmpleado; // Asigna el valor que venga del formulario

                // Insertamos el registro
                await _horasRepositorio.Add(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            // Si no es válido, recargamos el dropdown de empleados
            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View(horasTrabajo);
        }

        // GET: HorasTrabajo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            // Cargar la lista de empleados para el dropdown
            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View(horasTrabajo);
        }// Acción para confirmar la eliminación
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
