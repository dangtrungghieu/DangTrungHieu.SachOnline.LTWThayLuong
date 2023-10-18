using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.Data.Entity;
using DangTrungHieu.SachOnline.Models;
namespace DangTrungHieu.SachOnline.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        //Khai bao chuoi ket noi
        private SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/ChuDe
        public ActionResult Index()
        {
            return View(db.CHUDE.ToList());
        }
    }
}