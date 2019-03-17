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
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // check model state
            if(! ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                // declare slug
                string slug;

                PageDTO dto = new PageDTO();
                //DTO title

                dto.Title = model.Title;
                // check and sset slug if need
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace("", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();

                }
                // make sure title ans slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exist.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                    // save STO
                    db.Pages.Add(dto);
                db.SaveChanges();
            }
            // set tempData message
            TempData["SM"] = "You have added a new page!";
            // redirect
            return RedirectToAction("AddPage");
            }
        [HttpGet]
        // GET: Admin/Pages/EditPage/i
        public ActionResult EditPage(int id)
        {
            //Declare pagevm
            PageVM model;
            using (Db db = new Db())
            {
                // get page
                PageDTO dto = db.Pages.Find(id);
            
            // confirm page exist
            if (dto == null)
            {
                return Content("the oage doesn't exist.");
            }
            // init page vm
            model = new PageVM(dto);
        }

            //return view model
            return View(model);
        }
        [HttpPost]
        // POST: Admin/Pages/EditPage/i
        public ActionResult EditPage(PageVM model)
        {
            // check model state
        if( ! ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                // get page id
                int id = model.Id;

                //init slug
                string slug = "home";
                // get page
                PageDTO dto = db.Pages.Find(id);
                //DTO title
                dto.Title = model.Title;
                
                // check for slug ans set it if needed
                if (model.Slug != "home")
                    {
                    if(string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else 
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                    
                }
                // make sure slug and title are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exist.");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                // save dto
                db.SaveChanges();

            }
            // set tempdata message
            TempData["SM"] = "you have edited the page";

            // redirect
            return RedirectToAction("EditPage");

        }
        [HttpGet]
        // GET: Admin/Pages/PageDetails/i
        public ActionResult PageDetails(int id)
        {
            // declare pagevm
            PageVM model;

            using (Db db = new Db())
            {


                // get the page
                PageDTO dto = db.Pages.Find(id);
                // confirm the page
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }
                // init pagevm
                model = new PageVM(dto);
               
            }
            // return view with model
            return View(model);
        }
        // GET: Admin/Pages/DeletePage/i
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                // get the page
                PageDTO dto = db.Pages.Find(id);
                // remove page
                db.Pages.Remove(dto);

                //save
                db.SaveChanges();
            }
            // redirect
            return RedirectToAction("Index");
        }
        // POST: Admin/Pages/ReorderPages/i
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;

                // declare  pageDTO
                PageDTO dto;

                // sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }
    }
}
