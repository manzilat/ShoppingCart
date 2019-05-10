using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Declre list of models
            List<CategoryVM> categoryVMList;
            using (Db db = new Db())
            {
                //init list
                categoryVMList = db.Categories
                    .ToArray().
                    OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }


            //return view with list
            return View(categoryVMList);
        }
        // Post : Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //Declare Id
            string id;
            using (Db db = new Db())
            {
                // check cat name is unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";
            


            //init DTO
            CategoryDTO dto = new CategoryDTO();

            // add to dto
            dto.Name = catName;
            dto.Slug = catName.Replace(" ", "-").ToLower();
            dto.Sorting = 100;

            // save dto
            db.Categories.Add(dto);
            db.SaveChanges();

            //get the id
            id = dto.Id.ToString();
        }
            //return id
            return id;
        }
        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;

                // declare  categoryDTO
              CategoryDTO dto;

                // sorting for each category
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        // GET: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                // get the category
                CategoryDTO dto = db.Categories.Find(id);
                // remove categories
                db.Categories.Remove(dto);

                //save
                db.SaveChanges();
            }
            // redirect
            return RedirectToAction("Categories");
        }
    }
}