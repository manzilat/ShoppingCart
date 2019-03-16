using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // declare list of pagevm
            List<PageVM> pagesList;

            // init the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();

            }
            //Return view with list
            return View(pagesList);
        }
        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }
    }
}