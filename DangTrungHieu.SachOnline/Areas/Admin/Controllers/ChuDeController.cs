using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.Data.Entity;
using DangTrungHieu.SachOnline.Models;
using PagedList;
using System.IO;

namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        //Khai bao chuoi ket noi
        //private SachOnlineEntities db = new SachOnlineEntities();
        //// GET: Admin/ChuDe
        //public ActionResult Index(/*int? page*/)
        //{
        //    //if (Session["Admin"] == null || Session["Admin"].ToString() == "")
        //    //{
        //    //    return Redirect("~/Admin/Login/Login");
        //    //}
        //    //else
        //    //{

        //    //    int iPageNum = (page ?? 1);
        //    //    int iPageSize = 7;
        //    //    return View(db.CHUDE.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
        //    //}
        //    //if (Session["Admin"] == null || Session["Admin"].ToString() == "")
        //    //{
        //    //    return Redirect("~/Admin/Login/Login");
        //    //}
        //    //else
        //    //{
        //    //    return View();
        //    //}
        //    return View();
        //}
        //// GET: Admin/CHUDEs/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/CHUDEs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaCD,TenChuDe")] CHUDE cHUDE)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CHUDE.Add(cHUDE);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cHUDE);
        //}
        //// GET: Admin/CHUDEs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CHUDE cHUDE = db.CHUDE.Find(id);
        //    if (cHUDE == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cHUDE);
        //}

        //// POST: Admin/CHUDEs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "MaCD,TenChuDe")] CHUDE cHUDE)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cHUDE).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cHUDE);
        //}

        //// GET: Admin/CHUDEs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CHUDE cHUDE = db.CHUDE.Find(id);
        //    if (cHUDE == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cHUDE);
        //}

        //// POST: Admin/CHUDEs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CHUDE cHUDE = db.CHUDE.Find(id);
        //    db.CHUDE.Remove(cHUDE);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
    }
}
