﻿using ShoppingCart.Models.Data;
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
    }
}