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
            int ipageNum = (page ?? 1);
            int ipageSize = 7;
            return View(db.SACH.ToList().OrderBy(n => n.MaSach).ToPagedList(ipageNum,ipageSize));
        }
    }
}