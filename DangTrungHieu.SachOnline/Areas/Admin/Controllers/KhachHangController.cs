using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
using PagedList;

namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/KhachHang
        public ActionResult Index(int ?page)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Login/Login");
            }
            else
            {
                int iPageNum = (page ?? 1);
                int iPageSize = 7;

                return View(db.KHACHHANG.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
            }
            
        }
        // GET: Admin/KHACHHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANG.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // GET: Admin/KHACHHANGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTen,DiaChi,DienThoai,TaiKhoan,MatKhau,NgaySinh,Email")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANG.Add(kHACHHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kHACHHANG);
        }

        // GET: Admin/KHACHHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANG.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: Admin/KHACHHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTen,DiaChi,DienThoai,TaiKhoan,MatKhau,NgaySinh,Email")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACHHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHACHHANG);
        }

        // GET: Admin/KHACHHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANG.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: Admin/KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANG kHACHHANG = db.KHACHHANG.Find(id);
            db.KHACHHANG.Remove(kHACHHANG);
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