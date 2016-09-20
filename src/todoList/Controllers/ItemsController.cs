using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace todoList.Controllers
{
    public class ItemsController : Controller
    {
        private IItemRepository itemRepo;
        public ItemsController(IItemRepository thisRepo = null)
        {
            if (thisRepo == null)
            {
                this.itemRepo = new EFItemRepository();
            }
            else
            {
                this.itemRepo = thisRepo;
            }
        }
        public IActionResult Index()
        {
            return View(itemRepo.Items.Include(items => items.Category).ToList());
        }
        public IActionResult Details(int id)
        {
            var thisItem = itemRepo.Items.FirstOrDefault(items => items.ItemId == id);

            ViewBag.thisCategory = itemRepo.Categories.FirstOrDefault(categories => categories.CategoryId == thisItem.CategoryId);
 

            return View(thisItem);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(itemRepo.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            itemRepo.Save(item);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var thisItem = itemRepo.Items.FirstOrDefault(items => items.ItemId == id);
            ViewBag.CategoryId = new SelectList(itemRepo.Categories, "CategoryId", "Name");
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            itemRepo.Entry(item).State = EntityState.Modified;
            itemRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var thisItem = itemRepo.Items.FirstOrDefault(items => items .ItemId == id);
            return View(thisItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisItem = itemRepo.Items.FirstOrDefault(items => items .ItemId == id);
            itemRepo.Items.Remove(thisItem);
            itemRepo.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
