using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace TConsultigSA.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly PuestoRepositorio _puestoRepositorio;  // Repositorio de puestos
        private readonly DepartamentoRepositorio _departamentoRepositorio;  // Repositorio de departamentos

        public EmpleadosController(EmpleadoRepositorio empleadoRepositorio, PuestoRepositorio puestoRepositorio, DepartamentoRepositorio departamentoRepositorio)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _puestoRepositorio = puestoRepositorio;
            _departamentoRepositorio = departamentoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            return View(empleados);
        }

        // Nueva acción para obtener información completa del empleado
        public async Task<IActionResult> GetEmpleadoDetails(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return PartialView("_EmpleadoDetails", empleado);
        }

        public async Task<IActionResult> GetTablaCompleta()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            return PartialView("_EmpleadosTablaCompleta", empleados);
        }

        public async Task<IActionResult> Create()
        {
            await CargarPuestosYDepartamentos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Add(empleado);
                return RedirectToAction(nameof(Index));
            }

            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }

            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Update(empleado);
                return RedirectToAction(nameof(Index));
            }

            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empleadoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarPuestosYDepartamentos()
        {
            var puestos = await _puestoRepositorio.GetAll();
            var departamentos = await _departamentoRepositorio.GetAll();

            ViewBag.Puestos = puestos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Descripcion
            }).ToList();

            ViewBag.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DepartamentoNombre
            }).ToList();
        }
    }
}
