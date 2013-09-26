using GAExampleMVC.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAExampleMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var googleAnalytics = new GoogleAnalyticsService();

            ViewBag.Stats = googleAnalytics.GetStats();

            return View();
        }

    }
}
