using itemsproject.Models;
using itemsproject.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace itemsproject.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public CategoryController(IUnitOfWork _myunit)
        {
            myUnit = _myunit;
        }
        private IUnitOfWork myUnit;

        public async Task<IActionResult> Index()
        {
            return View(await myUnit.categories.GetAllAsync());
        }

        //AddMethode
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.ImageFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    await category.ImageFile.CopyToAsync(stream);
                    category.dbImage = stream.ToArray();
                }
                await myUnit.categories.AddAsync(category);
                return RedirectToAction("Index");
            }
            TempData["success"] = "Category created successfully";
            return View(category);
        }

        //EditMethode
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null && id == 0)
            {
                return NotFound();
            }
            var category = await myUnit.categories.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await myUnit.categories.UpdateAsync(category);
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //DeleteMethode
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null && id == 0)
            {
                return NotFound();
            }
            var category = await myUnit.categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await myUnit.categories.DeleteAsync(id);
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}