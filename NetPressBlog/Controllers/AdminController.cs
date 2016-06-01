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
    public class AdminController : Controller
    {
        private NetPressDBEntity1 db = new NetPressDBEntity1();

        // GET: Admin
        public ActionResult Index()
        {
            var blogInfoes = db.BlogInfoes.Include(b => b.AspNetUser).Include(b => b.Category);
            return View(blogInfoes.ToList());
        }

        // GET: Admin/Details/5
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

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Subtitle,Text,DateCreated,LastModified,Status,Category_Id,Author_Id")] BlogInfo blogInfo)
        {
            if (ModelState.IsValid)
            {
                db.BlogInfoes.Add(blogInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email", blogInfo.Author_Id);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Admin/Edit/5
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
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email", blogInfo.Author_Id);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // POST: Admin/Edit/5
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
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email", blogInfo.Author_Id);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Admin/Delete/5
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

        // POST: Admin/Delete/5
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
