using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TConsultigSA.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoRepositorio _prestamoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio; // Para seleccionar empleados

        public PrestamosController(PrestamoRepositorio prestamoRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _prestamoRepositorio = prestamoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        // Listar todos los préstamos
        public async Task<IActionResult> Index()
        {
            var prestamos = await _prestamoRepositorio.GetAll();
            return View(prestamos);
        }

        // Mostrar formulario de creación
        public async Task<IActionResult> Create()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View();
        }

        // Acción POST para crear un nuevo préstamo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                await _prestamoRepositorio.Add(prestamo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(prestamo);
        }

        // Mostrar formulario de edición
        public async Task<IActionResult> Edit(int id)
        {
            var prestamo = await _prestamoRepositorio.GetById(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(prestamo);
        }

        // Acción POST para editar un préstamo existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _prestamoRepositorio.Update(prestamo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(prestamo);
        }

        // Confirmar eliminación
        public async Task<IActionResult> Delete(int id)
        {
            var prestamo = await _prestamoRepositorio.GetById(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return View(prestamo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prestamoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
