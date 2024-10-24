using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class TiposPrestamoController : Controller
    {
        private readonly TipoPrestamoRepositorio _tipoPrestamoRepositorio;

        public TiposPrestamoController(TipoPrestamoRepositorio tipoPrestamoRepositorio)
        {
            _tipoPrestamoRepositorio = tipoPrestamoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var tiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View(tiposPrestamo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoPrestamo tipoPrestamo)
        {
            if (ModelState.IsValid)
            {
                await _tipoPrestamoRepositorio.Add(tipoPrestamo);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoPrestamo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tipoPrestamo = await _tipoPrestamoRepositorio.GetById(id);
            if (tipoPrestamo == null)
            {
                return NotFound();
            }

            return View(tipoPrestamo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoPrestamo tipoPrestamo)
        {
            if (id != tipoPrestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoPrestamoRepositorio.Update(tipoPrestamo);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoPrestamo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tipoPrestamo = await _tipoPrestamoRepositorio.GetById(id);
            if (tipoPrestamo == null)
            {
                return NotFound();
            }
            return View(tipoPrestamo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoPrestamoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
