using Microsoft.AspNetCore.Mvc;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using MvcCoreElastiCacheAWS.Services;

namespace MvcCoreElastiCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache service;

        public CochesController(RepositoryCoches repo, ServiceAWSCache service)
        {
            this.repo = repo;
            this.service = service;
        }

        public IActionResult Index()
        {
            List<Coche> coches = this.repo.GetCoches();
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche coche = this.repo.GetCoche(id);
            return View(coche);
        }

        public async Task<IActionResult> SeleccionarFavorito(int idcoche)
        {
            Coche coche = this.repo.GetCoche(idcoche);
            await this.service.AddCocheFavoritoAsync(coche);
            return RedirectToAction("Favoritos");
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> cochesFav = await this.service.GetCochesFavoritosAsync();
            return View(cochesFav);
        }

        public async Task<IActionResult> DeleteFavorito(int IdCoche)
        {
            await this.service.DeleteCocheFavoritoAsync(IdCoche);
            return RedirectToAction("Favoritos");
        }
    }
}
