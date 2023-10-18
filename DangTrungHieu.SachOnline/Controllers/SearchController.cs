using DangTrungHieu.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;
namespace DangTrungHieu.SachOnline.Controllers
{
    public class SearchController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Search
        /*public ActionResult Search(String strSearch)
        {
            var query = from s in db.SACHes
                        join cd in db.CHUDEs on s.MaCD equals cd.MaCD
                        where s.TenSach.Contains(strSearch) || cd.TenChuDe.Contains(strSearch) || s.NHAXUATBAN.TenNXB.Contains(strSearch)
                        select s;
            ViewBag.search = strSearch;
            query = query.OrderByDescending(b => b.SoLuongBan);
            return View(query.ToList());
        }*/

        public ActionResult Search(string strSearch = null, int maCD = 0)
        {
            ViewBag.Search = strSearch;
            var kq = db.SACH.Select(b => b);
            if (!String.IsNullOrEmpty(strSearch))
            {
                kq = kq.Where(b => b.TenSach.Contains(strSearch));
            }
            if(maCD != 0)
            {
                kq = kq.Where(b => b.CHUDE.MaCD == maCD);
            }

            ViewBag.MaCD = new SelectList(db.CHUDE, "MaCD", "TenChuDe");
            kq = kq.OrderBy(b => b.SoLuongBan);
            //kq = kq.OrderByDescending(b => b.NgayCapNhat);
            return View(kq.ToList());

        }

        public ActionResult Group()
        {
            var kq = db.SACH.GroupBy(s => s.MaCD);
            return View(kq);
        }

        public ActionResult ThongKe()
        {
            var kq = from s in db.SACH
                     join cd in db.CHUDE on s.MaCD equals cd.MaCD
                     group s by new { cd.MaCD, cd.TenChuDe } into g
                     select new ReportInfo
                     {
                         Id = g.Key.MaCD.ToString(),
                         Name = g.Key.TenChuDe,
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan)
                     };
            kq = kq.OrderByDescending(s => s.Sum);
            return View(kq);
        }
        public ActionResult SearchPhanTrang(int? page, string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!String.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                var kq = from s in db.SACH where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                return View(kq.OrderBy(s => s.MaSach).ToPagedList(iPageNumber, iSize));
            }
            return View();
        }
        public ActionResult SearchPhanTrangTuyChon(int ? size, int ? page, string strSearch = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3"});
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;
            ViewBag.Search = strSearch;

            int iSize = (size ?? 3);
            int iPageNumber = (page ?? 1);
            var kq = from s in db.SACH select s;
            if (!string.IsNullOrEmpty(strSearch))
            {
                kq = kq.Where(s => s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch));
            }
            return View(kq.OrderBy(s => s.MaSach).ToPagedList(iPageNumber, iSize));
        }
        public ActionResult SearchPhanTrangSapXep(int ? page, string sortProperty, string sortOder = "", string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                if (sortOder == "") ViewBag.SortOder = "desc";
                if (sortOder == "desc") ViewBag.SortOder = "";
                if (sortOder == "") ViewBag.SortOder = "asc";
                if (String.IsNullOrEmpty(sortProperty))
                {
                    sortProperty = "TenSach";
                }
                ViewBag.SortProperty = sortProperty;
                var kq = from s in db.SACH where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                if (sortOder == "desc")
                    kq = kq.OrderBy(sortProperty + " desc");
                else
                    kq = kq.OrderBy(sortProperty + " asc");
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();

        }
    }
}