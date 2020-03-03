using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_DAW.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Index()
        {
            var categories = from category in db.Categories
                             orderby category.name
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(int Id)
        {
            var category = db.Categories.Find(Id);
            ViewBag.CurrentCategory = category;

            var links = from link in db.ACLinks
                        where link.categoryId == Id
                        select link;
            int[] selectedArticles = new int[links.Count()];
            int i = 0;
            foreach (var link in links)
            {
                selectedArticles[i] = link.articleId;
                i++;
            }

            var articles = from article in db.Articles.Where(a => selectedArticles.Contains(a.articleId)).Where(a => a.last == true)
                           select article;
            ViewBag.Articles = articles;

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult New(Category cat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    var categories = from category in db.Categories
                                     orderby category.name
                                     select category;
                    ViewBag.Categories = categories;
                    return View("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}