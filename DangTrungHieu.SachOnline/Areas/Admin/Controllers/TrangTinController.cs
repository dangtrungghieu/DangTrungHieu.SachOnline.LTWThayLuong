using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
using SachOnline.Areas.Admin.Controllers;

namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class TrangTinController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/TrangTin
        public ActionResult Index()
        {
            return View(db.TRANGTIN.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TRANGTIN tt)
        {
            if (ModelState.IsValid)
            {
                tt.MetaTitle = tt.TenTrang.RemoveDiacritics().Replace(" ", "-");
                tt.NgayTao = DateTime.Now;
                db.TRANGTIN.Add(tt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tt = db.TRANGTIN.Where(t => t.MaTT == id);
            return View(tt.SingleOrDefault());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                int maTT = Convert.ToInt32(f["MaTT"]);
                var tt = db.TRANGTIN.Where(t => t.MaTT == maTT).SingleOrDefault();
                tt.TenTrang = f["TenTrang"];
                tt.NgayTao = Convert.ToDateTime(f["NgayTao"]);
                tt.NoiDung = f["NoiDung"];
                tt.MetaTitle = f["TenTrang"].RemoveDiacritics().Replace(" ", "-");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var tt = from t in db.TRANGTIN where t.MaTT == id select t;
            return View(tt.SingleOrDefault());
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var tt = (from t in db.TRANGTIN where t.MaTT == id select t).SingleOrDefault();
            db.TRANGTIN.Remove(tt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}