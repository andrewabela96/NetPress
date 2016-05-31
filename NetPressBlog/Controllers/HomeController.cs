using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetPressBlog.Models;
using System.Net;

namespace NetPressBlog.Controllers
{
    public class HomeController : Controller
    {
        private List<IQueryable> searchList = new List<IQueryable>();
        private NetPressDBEntity1 db = new NetPressDBEntity1();

        public ActionResult Index(string search)
        {

            if(!String.IsNullOrEmpty(search))
            {
                var posts = db.BlogInfoes.Where(p => p.Title.Contains(search) || (p.AspNetUser.UserName.Contains(search)) || p.Category.Type.Contains(search));
                //searchList.Add(posts);


                return View(posts);
            }
          
            var blogs = db.BlogInfoes.OrderByDescending(b => b.DateCreated);
            var blogPub = blogs.Where(b => b.Status == 1);
            return View(blogPub.ToList());
        }

        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogInfo blogInfo = db.BlogInfoes.Find(id);
            if (blogInfo == null)
            {
                return HttpNotFound();
            }
            return View(blogInfo);
        }

        public ActionResult Author(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blogInfo = db.BlogInfoes.Where(d => d.AspNetUser.UserName == id);
            var blogs = blogInfo.OrderByDescending(d => d.LastModified);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            return View(blogs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}