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
        private NetPressDBEntity db = new NetPressDBEntity();

        public ActionResult Index()
        {
          
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