using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DangTrungHieu.SachOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/LuotTruyCap.txt");
            System.IO.StreamReader stReader = new System.IO.StreamReader(filePath);

            string s = stReader.ReadLine();
            stReader.Close();
            Application.Add("HitCounter", s);
            Application["Online"] = 0;
        }
        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["Online"] = int.Parse(Application["Online"].ToString()) + 1;
            Application["HitCounter"] = int.Parse(Application["HitCounter"].ToString()) + 1;
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/LuotTruyCap.txt");
            System.IO.StreamWriter stWrite = new System.IO.StreamWriter(filePath);
            stWrite.Write(Application["HitCounter"]);
            stWrite.Close();
        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["Online"] = int.Parse(Application["Online"].ToString()) - 1;
            Application.UnLock();
        }
    }
}
