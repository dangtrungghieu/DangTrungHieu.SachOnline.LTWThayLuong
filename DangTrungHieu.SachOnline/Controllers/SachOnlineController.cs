using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DangTrungHieu.SachOnline.Models;
using System.Data.Linq;
using System.Web.Routing;
using System.Web.Optimization;
using System.Net;
using PagedList;
using System.Configuration;

namespace DangTrungHieu.SachOnline.Controllers
{
    
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        SachOnlineEntities db = new SachOnlineEntities();
        GioHangController gh = new GioHangController();
        public ActionResult SachOnline(int ?page)
        {
            int iSize = 6;
            int iPageNum = (page ?? 1);
            var sach = from s in db.SACH select s;

            return View(sach.OrderBy(s => s.MaSach).ToPagedList(iPageNum, iSize));
        }
        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult NXB()
        {
            var nxb = from data in db.NHAXUATBAN select data;
            return PartialView(nxb);
        }
        [ChildActionOnly]
        public ActionResult Slider()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Sachbannhieu()
        {
            var sachbannhieu = db.SACH.Take(6);
            return PartialView(sachbannhieu);
        }
        [ChildActionOnly]
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult ChuDePartial()
        {
            var chude = from data in db.CHUDE select data;
            return PartialView(chude);
        }
        [ChildActionOnly]
        public ActionResult DangNhapDangKyPartial()
        {
            return PartialView("DangNhapDangKyPartial");
        }
        [HttpGet]
        public ActionResult ChiTietSach(int id)
        {
            TempData["ReturnUrl"] = Request.Url.AbsoluteUri;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACH.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }
        //Chi tiet sach theo chu de

        [HttpGet]
        public ActionResult SachTheoChuDe(int id, int? page)
        {
            TempData["ReturnUrl"] = Request.Url.AbsoluteUri;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            ViewBag.MaCD = id;
            ViewBag.TenCD = db.CHUDE.Find(id).TenChuDe.ToString(); 
            var sach = from s in db.SACH where s.MaCD == id select s;
            return View(sach.OrderBy(s => s.MaCD).ToPagedList(iPageNum, iSize));
        }
        //Chi tiet sach theo nha xuat ban
        [HttpGet]
        public ActionResult SachTheoNXB(int id, int? page)
        {
            TempData["ReturnUrl"] = Request.Url.AbsoluteUri;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            ViewBag.MaNXB = id;
            ViewBag.TenNXB = db.NHAXUATBAN.Find(id).TenNXB.ToString();
            var sach = from s in db.SACH where s.MaNXB == id select s;
            return View(sach.OrderBy(s => s.MaNXB).ToPagedList(iPageNum, iSize));
        }

        //Thanh toán  qua VNPay
        public ActionResult Payment(int? id)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();


            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)

            pay.AddRequestData("vnp_Amount", gh.getTongTien().ToString() + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000

            pay.AddRequestData("vnp_BankCode", "NCB"); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Until.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }
}