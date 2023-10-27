using DangTrungHieu.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace DangTrungHieu.SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang == null)
            {
                //Khởi tạo giỏ hàng
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> listGioHang = LayGioHang();
            //Nếu giỏ hàng đã có sp thì tăng số lượng
            GioHang sp = listGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                listGioHang.Add(sp);

            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        int iTongSoLuong = 0;
        public int getTongSoLuong()
        {
            return iTongSoLuong;
        }
        private int TongSoLuong()
        {

            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(n => n.iSoLuong);

            }
            return iTongSoLuong;

        }

        static private double dTongTien = 0;
        public double getTongTien()
        {
            return dTongTien;
        }

        private double TongTien()
        {

            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang != null)
            {
                dTongTien = listGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }

        public ActionResult GioHang()
        {
            TempData["ReturnUrl"] = Request.Url.AbsoluteUri;
            List<GioHang> listGioHang = LayGioHang();
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("SachOnline", "SachOnline");

            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSPKhoiGiohang(int iMaSach)
        {
            List<GioHang> listGioHang = LayGioHang();
            //Kiem tra sach da co trong Session["GioHang"]
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                listGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (listGioHang.Count == 0)
                {
                    return RedirectToAction("SachOnline", "SachOnline");
                }

            }
            return RedirectToAction("GioHang");

        }

        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            //Nếu tồn tại số lượng thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGiohang()
        {
            List<GioHang> listGioHang = LayGioHang();
            listGioHang.Clear();
            return RedirectToAction("SachOnline", "SachOnline");

        }
        [HttpGet]
        public ActionResult DatHang()
        {
            
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap?id=2");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SachOnline", "SachOnline");
            }
            //Lấy giỏ hàng từ session
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);

        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f, Mail model)
        {
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                List<GioHang> listGioHang = LayGioHang();
                var NgayGiao = String.Format("{0:MM/mm/yyyy}", f["NgayGiao"]);
                ddh.MaKH = kh.MaKH;
                ddh.NgayDat = DateTime.Now;
                if (NgayGiao.ToString() == null)
                {
                ViewData["err"] = " Ngày giao hàng không phù hợp!";
                }
                else
                {
                    ddh.NgayGiao = DateTime.Parse(NgayGiao);
                }
                ddh.TinhTrangGiaoHang = 1;
                ddh.DaThanhToan = false;
                if (ddh.NgayGiao < ddh.NgayDat)
                {
                ViewData["err"] = " Ngày giao hàng không phù hợp!";
                }
                else {
                db.DONDATHANG.Add(ddh);
                db.SaveChanges();
                //Thêm chi tiết đơn hàng
                foreach (var item in listGioHang)
                {
                    CHITIETDATHANG ctdh = new CHITIETDATHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaSach = item.iMaSach;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    db.CHITIETDATHANG.Add(ctdh);

                }
                db.SaveChanges();
                var mail = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("dang.trungghieu@gmail.com", "addzwbzmfuidddbj"),
                    EnableSsl = true
                };
                var message = new MailMessage();
                message.IsBodyHtml = true;
                model.From = "dang.trungghieu@gmail.com";
                message.From = new MailAddress(model.From);
                message.ReplyToList.Add(model.From);
                model.To = kh.Email;
                message.To.Add(new MailAddress(model.To));
                model.Subject = "THÔNG TIN ĐẶT HÀNG";
                message.Subject = model.Subject;
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/GioHang/HoaDon.html"));
                string tableRows = ""; // Để lưu tất cả các thẻ <td>

                foreach (var item in listGioHang)
                {
                    string row = "<tr>";
                    row += "<td align=\"center\"><strong>" + item.iMaSach.ToString() + "</strong></td>";
                    row += "<td align=\"center\"><strong>" + item.sTenSach.ToString() + "</strong></td>";
                    row += "<td align=\"center\"><strong>" + item.iSoLuong.ToString() + "</strong></td>";
                    row += "<td align=\"center\"><strong>" + item.dDonGia.ToString() + "</strong></td>";
                    row += "<td align=\"center\"><strong>" + item.dThanhTien.ToString() + "</strong></td>";
                    row += "</tr>";

                    tableRows += row;
                }
                content = content.Replace("{{TableRows}}", tableRows);
                content = content.Replace("{{TongSoLuong}}", TongSoLuong().ToString());
                content = content.Replace("{{Hoten}}", kh.HoTen);
                content = content.Replace("{{Dienthoai}}", kh.DienThoai);
                content = content.Replace("{{Email}}", kh.Email);
                content = content.Replace("{{Diachi}}", kh.DiaChi);
                content = content.Replace("{{Ngaydat}}", ddh.NgayDat.ToString());
                content = content.Replace("{{Ngaygiao}}", ddh.NgayGiao.ToString());
                content = content.Replace("{{Tongtien}}", TongTien().ToString());
                content = content.Replace("{{mahoadon}}", ddh.MaDonHang.ToString());
                model.Notes = content.ToString();
                message.Body = model.Notes;
                mail.Send(message);
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }

            return this.DatHang();
        }

        public ActionResult XacNhanDonHang()
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            ViewBag.Email = kh.Email;
            return View();
        }

    }
}