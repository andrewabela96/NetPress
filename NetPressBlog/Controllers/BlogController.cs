using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPressBlog.Models;

namespace NetPressBlog.Controllers
{
    public class BlogController : Controller
    {
        private BlogsEntities db = new BlogsEntities();
        // GET: Blog
        public ActionResult Index()
        {
            var blogInfoes = db.BlogInfoes.Include(b => b.Category);
            return View(blogInfoes.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
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

        // GET: Blog/Create
        public ActionResult Create(string submit)
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type");
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Subtitle,Text,DateCreated,LastModified,Status,Category_Id,Author_Id")] BlogInfo blogInfo)
        {
            if (ModelState.IsValid)
            {
                string submit = Request["Submit"];
                switch(submit)
                {
                    case "Publish": blogInfo.Status = 1;
                                    break;
                    case "Draft":   blogInfo.Status = 2;
                                    break;
                    default:        blogInfo.Status = 2;
                                    break;
                }
                blogInfo.DateCreated = DateTime.Now;
                blogInfo.Author_Id = 1;
                db.BlogInfoes.Add(blogInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Subtitle,Text,DateCreated,LastModified,Status,Category_Id,Author_Id")] BlogInfo blogInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogInfo blogInfo = db.BlogInfoes.Find(id);
            db.BlogInfoes.Remove(blogInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
