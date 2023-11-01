using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
using PagedList.Mvc;
using System.IO;
using PagedList;

namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class SachController : Controller
    {
        // GET: Admin/Sach
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index(int ?page)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Login/Login");
            }
            else
            {
                int ipageNum = (page ?? 1);
                int ipageSize = 7;
                return View(db.SACH.ToList().OrderBy(n => n.MaSach).ToPagedList(ipageNum, ipageSize));
            }
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if(fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"];
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]); 
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    db.SACH.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANG.Where(ct => ct.MaSach == id);
            if(ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách hiện đang có trong bảng Chi Tiết Đặt Hàng <br>" + "Bạn vẫn muốn xóa quyển sách này chứ?";
                return View(sach);
            }
            var vietsach = db.VIETSACH.Where(vs => vs.MaSach == id).ToList();
            if(vietsach != null)
            {
                db.VIETSACH.RemoveRange(vietsach);
                db.SaveChanges();
            }
            db.SACH.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit( FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var maSach = int.Parse(f["iMaSach"]);
            var sach = db.SACH.SingleOrDefault(n => n.MaSach == maSach );
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            if (ModelState.IsValid)
            {
                if(fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }
                sach.TenSach = f["sTenSach"];
                sach.MoTa = f["sMoTa"];
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                sach.MaCD = int.Parse(f["MaCD"]);
                sach.MaNXB = int.Parse(f["MaNXB"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}