using itemsproject.Data;
using itemsproject.Models;
using itemsproject.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace itemsproject.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        public ItemsController(IUnitOfWork _myunit, IWebHostEnvironment host)
        {
            myUnit = _myunit;
            _host = host;
        }
        private readonly IUnitOfWork myUnit;
        private readonly IWebHostEnvironment _host;

        public async Task<IActionResult> Index()
        {
            IEnumerable<Item> itemslist = await myUnit.items.GetAllAsync(includeProperties: "Category");
            return View(itemslist);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Selectedlist();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if (item.ImageFile != null)
                {
                    string myOpload = Path.Combine(_host.WebRootPath, "images");
                    fileName = Path.GetFileName(item.ImageFile.FileName);
                    string fullPath = Path.Combine(myOpload, fileName);
                    Directory.CreateDirectory(myOpload);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        item.ImageFile.CopyTo(stream);
                    }
                    item.ImagePath = fileName;
                }
                await myUnit.items.AddAsync(item);
                TempData["success"] = "Item created successfully";
                return RedirectToAction("Index");

            }
            else
            {
                return View(item);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = await myUnit.items.GetByIdAsync(Id.Value);
            if (item == null) return NotFound();
            await Selectedlist(item.CategoryId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                await myUnit.items.UpdateAsync(item);
                TempData["success"] = "Item edited successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = await myUnit.items.GetByIdAsync(Id.Value);
            if (item == null) return NotFound();
            Selectedlist(item.CategoryId);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int? Id)
        {
            var item = await myUnit.items.GetByIdAsync(Id.Value);
            if (item == null)
            {
                return NotFound();
            }
            await myUnit.items.DeleteAsync(item.Id);
            await myUnit.CompleteAsync();
            TempData["success"] = "Item deleted successfully";
            return RedirectToAction("Index");
        }

        public async Task Selectedlist(int selectId = 1)
        {
            var categories = (await myUnit.categories.GetAllAsync()).ToList();
            SelectList listselected = new SelectList(categories, "Id", "Name", selectId);
            ViewBag.categorylist = listselected;
        }
    }
}