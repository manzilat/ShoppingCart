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
    }
}