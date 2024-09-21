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

        // Constructor que recibe los repositorios de empleados, puestos y departamentos
        public EmpleadosController(EmpleadoRepositorio empleadoRepositorio, PuestoRepositorio puestoRepositorio, DepartamentoRepositorio departamentoRepositorio)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _puestoRepositorio = puestoRepositorio;
            _departamentoRepositorio = departamentoRepositorio;
        }

        // Acción para mostrar la lista de empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            return View(empleados);
        }

        // Acción para mostrar el formulario de creación (GET)
        public async Task<IActionResult> Create()
        {
            // Cargar los puestos y departamentos para el formulario
            await CargarPuestosYDepartamentos();
            return View();
        }

        // Acción para manejar el POST del formulario de creación (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Add(empleado);  // Agregar el nuevo empleado
                return RedirectToAction(nameof(Index));  // Redirigir a la acción Index
            }

            // Volver a cargar las listas si hay un error en la validación
            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        // Acción para mostrar el formulario de edición (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();  // Si no se encuentra el empleado, devolver error 404
            }

            // Cargar los puestos y departamentos para la edición
            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        // Acción para manejar el POST del formulario de edición (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();  // Si el ID no coincide, devolver error 404
            }

            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Update(empleado);  // Actualizar el empleado en la base de datos
                return RedirectToAction(nameof(Index));  // Redirigir a la acción Index
            }

            // Volver a cargar las listas si hay un error en la validación
            await CargarPuestosYDepartamentos();
            return View(empleado);
        }

        // Acción para confirmar la eliminación (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();  // Si no se encuentra el empleado, devolver error 404
            }
            return View(empleado);
        }

        // Acción para manejar la eliminación (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empleadoRepositorio.Delete(id);  // Eliminar el empleado de la base de datos
            return RedirectToAction(nameof(Index));  // Redirigir a la acción Index
        }

        // Método auxiliar para cargar los puestos y departamentos
        private async Task CargarPuestosYDepartamentos()
        {
            var puestos = await _puestoRepositorio.GetAll();
            var departamentos = await _departamentoRepositorio.GetAll();

            ViewBag.Puestos = puestos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),       // Usar la propiedad Id de tu modelo Puesto
                Text = p.Descripcion           // Usar la propiedad Descripcion de tu modelo Puesto
            }).ToList();

            ViewBag.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),       // Usar la propiedad Id de tu modelo Departamento
                Text = d.DepartamentoNombre    // Usar la propiedad DepartamentoNombre de tu modelo Departamento
            }).ToList();
        }


    }
}
