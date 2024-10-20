using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly EmpresaRepositorio _empresaRepositorio;

        public EmpresasController(EmpresaRepositorio empresaRepositorio)
        {
            _empresaRepositorio = empresaRepositorio;
        }

        // Acción para listar todas las empresas
        public async Task<IActionResult> Index()
        {
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();
            return View(empresas);  // Asegúrate de tener la vista en /Views/Empresas/Index.cshtml
        }

        // Acción GET para mostrar el formulario de creación de empresa
        public IActionResult Create()
        {
            return View();
        }

        // Acción POST para crear una nueva empresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                await _empresaRepositorio.AddEmpresaAsync(empresa);
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // Acción GET para editar una empresa
        public async Task<IActionResult> Edit(int id)
        {
            var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // Acción POST para actualizar una empresa existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _empresaRepositorio.UpdateEmpresaAsync(empresa);
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // Acción GET para mostrar la confirmación de eliminación de empresa
        public async Task<IActionResult> Delete(int id)
        {
            var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // Acción POST para eliminar una empresa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empresaRepositorio.DeleteEmpresaAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
