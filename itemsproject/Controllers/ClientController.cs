using itemsproject.Models;
using itemsproject.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace itemsproject.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private IUnitOfWork myUnit;
        public ClientController(IUnitOfWork _myunit)
        {
            myUnit = _myunit;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await myUnit.clients.GetAllAsync(includeProperties: "Items.Category");
            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var items = await myUnit.items.GetAllAsync();
            ViewBag.Items = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Client client, int[] selectedItems)
        {
            if (ModelState.IsValid)
            {
                // Récupère uniquement les Item existants et les attache au client
                var items = (await myUnit.items.GetByIdsAsync(selectedItems)).ToList();
                client.Items = items;

                await myUnit.clients.AddAsync(client);
                TempData["success"] = "Client created successfully";
                return RedirectToAction("Index");
            }

            var allItems = await myUnit.items.GetAllAsync();
            ViewBag.Items = allItems;
            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = await myUnit.clients.GetByIdAsync(id, includeProperties: "Items.Category");
            if (client == null)
            {
                return NotFound();
            }

            var allItems = await myUnit.items.GetAllAsync();
            ViewBag.Items = allItems;

            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client, int[] selectedItems)
        {
            if (!ModelState.IsValid)
            {
                var allItems = await myUnit.items.GetAllAsync();
                ViewBag.Items = allItems;
                return View(client);
            }

            var existingClient = await myUnit.clients.GetByIdAsync(client.Id, includeProperties: "Items");
            if (existingClient == null)
            {
                return NotFound();
            }
            existingClient.Name = client.Name;
            var items = (selectedItems != null && selectedItems.Length > 0)
                ? (await myUnit.items.GetByIdsAsync(selectedItems)).ToList()
                : new List<Item>();

            existingClient.Items = items;

            await myUnit.clients.UpdateAsync(existingClient);
            TempData["success"] = "Client edited successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await myUnit.clients.GetByIdAsync(id, includeProperties: "Items.Category");
            if (client == null)
            {
                TempData["error"] = "Client not found";
                return RedirectToAction("Index");
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await myUnit.clients.GetByIdAsync(id, includeProperties: "Items");
            if (client == null)
            {
                TempData["error"] = "Client not found";
                return RedirectToAction("Index");
            }

            await myUnit.clients.DeleteAsync(client.Id);
            TempData["success"] = "Client deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
