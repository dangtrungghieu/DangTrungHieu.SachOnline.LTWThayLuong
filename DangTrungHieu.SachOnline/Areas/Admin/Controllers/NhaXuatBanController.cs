using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DangTrungHieu.SachOnline.Models;
using System.Data.Linq;
using PagedList;
using System.Net;
using System.Data;

namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        // GET: Admin/NhaXuatBan
        private SachOnlineEntities db1 = new SachOnlineEntities();
        public ActionResult Index()
        {
                return View(db1.NHAXUATBAN.ToList());
        }
        // GET: Admin/NhaXuatBan/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MaNXB,TenNXB,DiaChi,DienThoai")] NHAXUATBAN nHAXUATBAN, FormCollection f)
        {
            var tenNXB = f["TenNXB"];
            var dienThoai = f["DienThoai"];
            Validate(tenNXB, dienThoai);
            if (ModelState.IsValid)
            {
                db1.NHAXUATBAN.Add(nHAXUATBAN);
                db1.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nHAXUATBAN);
        }
        // GET: Admin/NhaXuatBan/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db1.NHAXUATBAN.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NHAXUATBAN nHAXUATBAN = db1.NHAXUATBAN.Find(id);
            db1.NHAXUATBAN.Remove(nHAXUATBAN);
            db1.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/NhaXuatBan/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db1.NHAXUATBAN.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }
        // GET: Admin/NHAXUATBANs/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db1.NHAXUATBAN.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNXB,TenNXB,DiaChi,DienThoai")] NHAXUATBAN nHAXUATBAN)
        {
            if (ModelState.IsValid)
            {
                db1.Entry(nHAXUATBAN).State = EntityState.Modified;
                db1.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nHAXUATBAN);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db1.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Validate(String tenNXB, String dienThoai)
        {
            if (String.IsNullOrEmpty(tenNXB))
            {
                ModelState.AddModelError("TenNXB", "Không được để trống.");
            }
            else if (db1.NHAXUATBAN.Where(n=>n.TenNXB == tenNXB).SingleOrDefault() != null)
            {
                ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại.");
            }
            else if (String.IsNullOrEmpty(dienThoai))
            {
                ModelState.AddModelError("dienThoai", "Không được để trống.");
            }
            return View("Create");
        }
    }
}