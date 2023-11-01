using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/Home
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
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
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];
            ADMIN ad = db.ADMIN.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không đúng !";
            }
            return View();
        }

    }
}