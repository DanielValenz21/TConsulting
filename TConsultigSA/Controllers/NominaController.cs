using Microsoft.AspNetCore.Mvc;
using TConsultigSA.Services;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class NominaController : Controller
    {
        private readonly NominaService _nominaService;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public NominaController(NominaService nominaService, EmpleadoRepositorio empleadoRepositorio)
        {
            _nominaService = nominaService;
            _empleadoRepositorio = empleadoRepositorio;
        }

        public async Task<IActionResult> CalcularNomina()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            ViewBag.Empleados = empleados;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalcularNomina(int idEmpleado, int mes, int año)
        {
            try
            {
                var resultado = await _nominaService.CalcularNominaParaEmpleado(idEmpleado, mes, año);
                return View("ResultadoNomina", resultado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var empleados = await _empleadoRepositorio.GetAll();
                ViewBag.Empleados = empleados;
                return View();
            }
        }
    }
}
