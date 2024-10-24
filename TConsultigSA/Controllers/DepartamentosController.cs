using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace TConsultigSA.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly DepartamentoRepositorio _departamentoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly EmpresaRepositorio _empresaRepositorio;

        public DepartamentosController(DepartamentoRepositorio departamentoRepositorio,
                                       EmpleadoRepositorio empleadoRepositorio,
                                       EmpresaRepositorio empresaRepositorio)
        {
            _departamentoRepositorio = departamentoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _empresaRepositorio = empresaRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoRepositorio.GetAll();
            var viewModelList = new List<DepartamentoViewModel>();

            foreach (var departamento in departamentos)
            {
                var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(departamento.IdEmpresa);
                var lider = await _empleadoRepositorio.GetById(departamento.IdLider ?? 0);

                var viewModel = new DepartamentoViewModel
                {
                    Id = departamento.Id,
                    DepartamentoNombre = departamento.DepartamentoNombre,
                    EmpresaNombre = empresa?.Nombre ?? "N/A",
                    LiderNombre = lider?.Nombre ?? "Sin Líder"
                };

                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        public async Task<IActionResult> Create()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();

            // Convertir empleados a SelectListItem
            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            // Convertir empresas a SelectListItem
            ViewBag.Empresas = empresas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                await _departamentoRepositorio.Add(departamento);
                return RedirectToAction(nameof(Index));
            }

            // Volver a cargar empleados y empresas en caso de error en la validación
            ViewBag.Empleados = (await _empleadoRepositorio.GetAll())
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre })
                .ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync())
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre })
                .ToList();

            return View(departamento);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var departamento = await _departamentoRepositorio.GetById(id);
            if (departamento == null)
            {
                return NotFound();
            }

            // Cargar empleados y empresas
            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _departamentoRepositorio.Update(departamento);
                return RedirectToAction(nameof(Index));
            }

            // Si hay un error en el modelo, volver a cargar las listas de empleados y empresas
            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(departamento);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var departamento = await _departamentoRepositorio.GetById(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departamentoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarLideresYEmpresas()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();

            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = empresas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();
        }

    }
}