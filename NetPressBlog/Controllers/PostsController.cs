using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPressBlog.Models;
using Microsoft.AspNet.Identity;

namespace NetPressBlog.Controllers
{
    public class PostsController : Controller
    {
        private NetPressDBEntity1 db = new NetPressDBEntity1();

        // GET: Posts
        [Authorize]
        public ActionResult Index()
        {

            var blogInfo = db.BlogInfoes.Include(b => b.Category);
            string currUser = User.Identity.GetUserId();
            var user = blogInfo.Where(b => b.Author_Id == currUser);/*from b in blogInfoes
                       where (b.Author_Id == User.Identity.GetUserId())
                       select b;*/

            return View(user.ToList());
        }

        // GET: Posts/Details/5
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

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create(string submit)
        {
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type");
            return View();
        }

        // POST: Posts/Create
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
                switch (submit)
                {
                    case "Publish":
                        blogInfo.Status = 1;
                        break;
                    case "Draft":
                        blogInfo.Status = 2;
                        break;
                    default:
                        blogInfo.Status = 2;
                        break;
                }
                blogInfo.DateCreated = DateTime.Now;
                blogInfo.Author_Id = User.Identity.GetUserId();
                db.BlogInfoes.Add(blogInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogInfo blogInfo = db.BlogInfoes.Find(id);
            if ((blogInfo == null) || (blogInfo.Author_Id != User.Identity.GetUserId())) //Checks if there is 
            {
                return HttpNotFound();
            }
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email", blogInfo.Author_Id);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Subtitle,Text,DateCreated,LastModified,Status,Category_Id,Author_Id")] BlogInfo blogInfo)
        {
            if (ModelState.IsValid)
            {
                blogInfo.LastModified = DateTime.Now;
                db.Entry(blogInfo).State = EntityState.Modified;
          
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Author_Id = new SelectList(db.AspNetUsers, "Id", "Email", blogInfo.Author_Id);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Type", blogInfo.Category_Id);
            return View(blogInfo);
        }

        // GET: Posts/Delete/5
        [Authorize]
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

        // POST: Posts/Delete/5
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
