using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/Menu
        public ActionResult Index()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Login/Login");
            }
            else
            {
                var listMenu = db.MENU.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
                int[] a = new int[listMenu.Count()];
                for (int i = 0; i < listMenu.Count; i++)
                {
                    var temp = (int)listMenu[i].Id;
                    a[i] = db.MENU.Count(m => m.ParentId == temp);
                }
                ViewBag.lst = a;
                List<CHUDE> cd = db.CHUDE.ToList();
                ViewBag.ChuDe = cd;
                List<TRANGTIN> tt = db.TRANGTIN.ToList();
                ViewBag.TrangTin = tt;
                return View("Index",listMenu);
            }
        }

        [ChildActionOnly]
        public ActionResult ChildMenu(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = db.MENU.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var temp = (int)lst[i].Id;
                a[i] = db.MENU.Count(m => m.ParentId == temp);
            }
            ViewBag.lst = a;

            return PartialView("ChildMenu", lst);
        }
        [ChildActionOnly]
        public ActionResult ChildMenu1(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = db.MENU.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var temp = (int)lst[i].Id;
                a[i] = db.MENU.Count(m => m.ParentId == temp);
            }
            ViewBag.lst = a;

            return PartialView("ChildMenu1", lst);
        }
        [HttpPost]
        public ActionResult AddMenu(FormCollection f)
        {
            if (!String.IsNullOrEmpty(f["ThemChuDe"]))
            {
                MENU m = new MENU();
                var temp = int.Parse(f["MaCD"].ToString());
                var cd = db.CHUDE.Where(c => c.MaCD ==temp ).SingleOrDefault();
                m.MenuName = cd.TenChuDe;
                m.MenuLink = "sach-theo-chu-de-" + cd.MaCD;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number"]);
                List<MENU> l = null;
                if(m.ParentId == null)
                {
                    l = db.MENU.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();

                }
                else
                {
                    l = db.MENU.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for(int i = 0; i< l.Count; i++)
                {
                    l[i].OrderNumber++;
                }
                db.MENU.Add(m);
                db.SaveChanges();
            }else if(!String.IsNullOrEmpty(f["ThemTrang"])){
                MENU m = new MENU();
                var temp = int.Parse(f["MaTT"].ToString());
                var trang = db.TRANGTIN.Where(t => t.MaTT == temp).SingleOrDefault();
                m.MenuName = trang.TenTrang;
                m.MenuLink = trang.MetaTitle;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number1"]);
                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = db.MENU.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();

                }
                else
                {
                    l = db.MENU.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber++;
                }
                db.MENU.Add(m);
                db.SaveChanges();
            }else if(!String.IsNullOrEmpty(f["ThemLink"])){
                MENU m = new MENU();
                m.MenuName = f["TenMenu"];
                m.MenuLink = f["Link"];
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number2"]);
                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = db.MENU.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();

                }
                else
                {
                    l = db.MENU.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber++;
                }
                db.MENU.Add(m);
                db.SaveChanges();
            }
            return Redirect("~/Admin/Menu/Index");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            List<MENU> submn = db.MENU.Where(m => m.ParentId == id).ToList();
            if(submn.Count() > 0)
            {
                return Json(new { code = 500, msg = "Còn menu con, không xóa được." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var mn = db.MENU.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if(mn.ParentId == null)
                {
                    l = db.MENU.Where(k => k.ParentId == null && k.OrderNumber > mn.OrderNumber).ToList();
                    
                }
                else
                {
                    l = db.MENU.Where(k => k.ParentId == mn.ParentId && k.OrderNumber > mn.OrderNumber).ToList();
                }
                for(int i =0; i < l.Count; i++)
                {
                    l[i].OrderNumber--;
                }
                db.MENU.Remove(mn);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            try
            {
                var mn = (from m in db.MENU where (m.Id == id) select new
                {
                    iD = m.Id,
                    MenuName = m.MenuName,
                    MenuLink = m.MenuLink,
                    OrderNumber = m.OrderNumber
                }).SingleOrDefault();
                return Json(new { code = 200, mn = mn ,msg = "Lấy thông tin thành công." }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult Update(int id, string strTenMenu, string strLink, int STT)
        {
            try
            {
                var mn = db.MENU.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if(STT < mn.OrderNumber)
                {
                    if (mn.ParentId == null)
                        l = db.MENU.Where(m => m.ParentId == null && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    else
                        l = db.MENU.Where(m => m.ParentId == mn.ParentId && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    for(int i = 0; i< l.Count; i++)
                    {
                        l[i].OrderNumber++;
                    }
                }else if(STT > mn.OrderNumber){
                    if (mn.ParentId == null)
                        l = db.MENU.Where(m => m.ParentId == null && m.OrderNumber > STT && m.OrderNumber <= STT).ToList();
                    else
                        l = db.MENU.Where(m => m.ParentId == mn.ParentId && m.OrderNumber > STT && m.OrderNumber <= STT).ToList();
                    for (int i = 0; i < l.Count; i++)
                    {
                        l[i].OrderNumber--;
                    }
                }
                mn.MenuName = strTenMenu;
                mn.MenuLink = strLink;
                mn.OrderNumber = STT;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa thông tin thành công." }, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa thông tin thất bại." + ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}