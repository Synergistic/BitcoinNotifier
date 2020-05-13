using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnimalCrossingPrices.Models;
using BitcoinNotifier.Services.Interface;

namespace AnimalCrossingPrices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnimalCrossingStorageService AnimalCrossingStorageService;


        public HomeController(ILogger<HomeController> logger, IAnimalCrossingStorageService animalCrossingStorageService)
        {
            _logger = logger;
            this.AnimalCrossingStorageService = animalCrossingStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await AnimalCrossingStorageService.GetAllItems();
            return View(items);
        }

        public async Task<IActionResult> Add(string itemName, string price)
        {
            if(Int32.TryParse(price, out int priceValue))
            {
                await AnimalCrossingStorageService.AddPriceForItem(itemName, priceValue);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string itemName)
        {
            await AnimalCrossingStorageService.RemoveItemByName(itemName);
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
