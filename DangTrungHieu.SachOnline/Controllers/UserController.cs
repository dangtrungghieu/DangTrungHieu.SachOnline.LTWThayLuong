using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DangTrungHieu.SachOnline.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace DangTrungHieu.SachOnline.Controllers
{
    public class UserController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var s1MatKhau = HashPassword(collection["MatKhau"]);
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == s1MatKhau);
                if(kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                    if (TempData["ReturnUrl"] != null)
                    {
                        string returnUrl = TempData["ReturnUrl"].ToString();
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("SachOnline", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];
            var sTaiKhoan = collection["TaiKhoan"];
            var sMatkhau = collection["MatKhau"];
            var sMatkhauNhapLai =collection["MatkhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:y yy yyy yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sMatkhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu"; 
            }
            else if (sMatkhau != sMatkhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if(db.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan) != null)
            {
                ViewBag.ThongBao = "Tên tài khoản đã tồn tại";
            }
            else if (db.KHACHHANG.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else if (ModelState.IsValid)
            {
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTaiKhoan;
                kh.MatKhau = HashPassword(sMatkhau);
                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANG.Add(kh);
                db.SaveChanges();
                return Redirect("~/User/DangNhap");
            }
            return View("DangKy");
        }

        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(FormCollection collection, KHACHHANG kh)
        //{
        //    //// Gan cac gia tri nguoi dung nhap du lieu cho cac bien
        //    //var sHoTen = collection["HoTen"];
        //    //var sTenDN = collection["TaiKhoan"];
        //    //var sMatkhau = collection["MatKhau"];
        //    //var sMatkhauNhapLai = collection["MatkhauNL"];
        //    //var sDiaChi = collection["DiaChi"];
        //    //var sEmail = collection["Email"];
        //    //var sDienThoai = collection["DienThoai"];
        //    //var dNgaySinh = String.Format("{0:y yy yyy yyyy}", collection["NgaySinh"]);

        //    //if (String.IsNullOrEmpty(sHoTen))
        //    //{
        //    //    ViewData["err1"] = " Họ tên không được rỗng ";
        //    //}
        //    //else if (String.IsNullOrEmpty(sTenDN))
        //    //{
        //    //    ViewData["err2"] = " Tên đăng nhập không được rỗng ";
        //    //}
        //    //else if (String.IsNullOrEmpty(sMatkhau))
        //    //{
        //    //    ViewData[" err3 "] = "Phải nhập mật khẩu";
        //    //}
        //    //else if (String.IsNullOrEmpty(sMatkhauNhapLai))
        //    //{
        //    //    ViewData[" err4 "] = " Phải nhập lại mật khẩu ";
        //    //}
        //    //else if (sMatkhau != sMatkhauNhapLai)
        //    //{
        //    //    ViewData[" err4 "] = " Mật khẩu nhập lại không khớp ";
        //    //}
        //    //else if (String.IsNullOrEmpty(sEmail))
        //    //{
        //    //    ViewData[" err5 "] = " Email không được rỗng ";
        //    //}
        //    //else if (String.IsNullOrEmpty(sDienThoai))
        //    //{
        //    //    ViewData[" err6 "] = " Số điện thoại không được rỗng ";
        //    //}
        //    //else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
        //    //{
        //    //    ViewBag.ThongBao = " Tên tài khoản đã tồn tại ";
        //    //}
        //    //else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
        //    //{
        //    //    ViewBag.ThongBao = " Email đã được sử dụng ";
        //    //}
        //    //else
        //    //{
        //    //    // Gán giá trị cho đối tượng được tạo mới ( kh )
        //    //    kh.HoTen = sHoTen;
        //    //    kh.TaiKhoan = sTenDN;
        //    //    kh.MatKhau = sMatkhau;
        //    //    kh.Email = sEmail;
        //    //    kh.DiaChi = sDiaChi;
        //    //    kh.DienThoai = sDienThoai;
        //    //    kh.NgaySinh = DateTime.Parse(dNgaySinh);
        //    //    db.KHACHHANGs.Add(kh);
        //    //    db.SaveChanges();
        //    //    return Redirect("~/User/DangNhap");
        //    //}
        //    //return this.Register();
        //}

    }
}