using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_DAW.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Index()
        {
            var articles = from article in db.Articles.Include("User")
                           where article.last == true
                           orderby article.date descending
                           select article;
            ViewBag.Articles = articles;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            var categories = from category in db.Categories
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPost]
        public ActionResult New(Article art, int[] categoryId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    art.date = DateTime.Now;
                    art.last = true;
                    art.userId = User.Identity.GetUserId();

                    //sets "last" parameter to false for all other versions
                    var previousVersions = from article in db.Articles
                                           where article.title == art.title
                                           select article;
                    foreach (Article a in previousVersions)
                    {
                        a.last = false;
                    }

                    db.Articles.Add(art);
                    db.SaveChanges(); //aici articolul obtine un ID
                    foreach (var i in categoryId)
                    {
                        LinkAC l = new LinkAC();
                        l.categoryId = i;
                        l.articleId = art.articleId;
                        db.ACLinks.Add(l);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var categories = from category in db.Categories
                                     select category;
                    ViewBag.Categories = categories;
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(int Id)
        {
            Article art = db.Articles.Find(Id);
            ViewBag.CurrentArticle = art;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int Id)
        {
            Article art = db.Articles.Find(Id);
            if (art.userId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                ViewBag.CurrentArticle = art;
                return View();
            } else
            {
                return RedirectToAction("Index");
            }    
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPost]
        public ActionResult Edit(int Id, Article newArt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newArt.date = DateTime.Now;
                    newArt.last = true;
                    newArt.userId = User.Identity.GetUserId();

                    //sets "last" parameter to false for all other versions
                    var previousVersions = from article in db.Articles
                                           where article.title == newArt.title
                                           select article;
                    foreach (Article a in previousVersions)
                    {
                        a.last = false;
                    }

                    db.Articles.Add(newArt);
                    db.SaveChanges();

                    //copy article categories to new version
                    var prevArtCategories = from link in db.ACLinks
                                            where link.articleId == Id
                                            select link;
                    foreach (LinkAC l in prevArtCategories)
                    {
                        LinkAC newLink = new LinkAC();
                        newLink.categoryId = l.categoryId;
                        newLink.articleId = newArt.articleId;
                        db.ACLinks.Add(newLink);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Article art = db.Articles.Find(Id);
                    ViewBag.CurrentArticle = art;
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPut]
        public ActionResult RollbackReplace(string artTitle, int artId)
        {
            var prevVersions = from article in db.Articles.Where(a => a.title == artTitle).Where(a => a.last == false)
                               orderby article.date descending
                               select article;

            if (prevVersions.Any())
            {
                Article penultimate = prevVersions.First();
                if (TryUpdateModel(penultimate))
                {
                    penultimate.last = true;
                    Article a = db.Articles.Find(artId);
                    db.Articles.Remove(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /*
        [HttpDelete]
        public ActionResult RollbackDelete(int currentId, int previousId)
        {
            Article art = db.Articles.Find(currentId);
            db.Articles.Remove(art);
            db.SaveChanges();
            return RedirectToAction("Show", previousId);
        }
        */

        [Authorize(Roles = "Editor,Administrator")]
        [HttpDelete]
        public ActionResult Delete(string title)
        {
            var articles = from article in db.Articles
                           where article.title == title
                           select article;
            foreach (Article art in articles)
            {
                db.Articles.Remove(art);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Search(string searchTitle)
        {
            var articles = from article in db.Articles.Where(a => a.title.Contains(searchTitle))
                           select article;
            ViewBag.SelectedArticles = articles;
            return View();
        }
    }
}