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
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Login/Login");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public JsonResult DsChuDe(int page = 1, int pageSize = 5)
        {
            try
            {
                var totalChuDe = db.CHUDE.Count();
                var dsCD = db.CHUDE
                    .OrderBy(c => c.MaCD)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(cd => new
                    {
                        MaCD = cd.MaCD,
                        TenCD = cd.TenChuDe
                    })
                    .ToList();

                var totalPages = (int)Math.Ceiling((double)totalChuDe / pageSize);

                return Json(new
                {
                    code = 200,
                    dsCD = dsCD,
                    msg = "Lấy danh sách thành công!",
                    currentPage = page,
                    totalPages = totalPages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Detail(int maCD)
        {
            try
            {
                var cd = (from s in db.CHUDE where (s.MaCD == maCD)
                          select new {
                              MaCD = s.MaCD,
                              TenChuDe = s.TenChuDe
                          }).SingleOrDefault();
                return Json(new { code = 200, cd = cd, msg = "Lay thong tin thanh cong" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD;
                db.CHUDE.Add(cd);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Them thanh cong" }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Them that bai" + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult Update(int maCD, string strTenCD)
        {
            try
            {
                var cd = db.CHUDE.SingleOrDefault(c => c.MaCD == maCD);
                cd.TenChuDe = strTenCD;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sua thanh cong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sua that bai" + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult Delete(int maCD)
        {
            try
            {
                var cd = db.CHUDE.SingleOrDefault(c => c.MaCD == maCD);
                db.CHUDE.Remove(cd);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xoa thanh cong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xoa that bai" + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
