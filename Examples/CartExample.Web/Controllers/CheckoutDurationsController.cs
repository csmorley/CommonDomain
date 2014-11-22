using CartExample.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CartExample.Web.Controllers
{
    public class CheckoutDurationsController : Controller
    {
        public CheckoutDurationsController(Database readModel)
        {
            this.readModel = readModel;
        }

        Database readModel;

        // GET: LongestCheckoutDurations
        public ActionResult Index(double seconds = 600)
        {
            var durations = this.readModel.CheckoutDurations.Where(x => x.Value > seconds);
            durations.OrderByDescending( x => x.Value);
            ViewBag.Data = durations;
            ViewBag.Seconds = seconds;
            ViewBag.Total = durations.Count();
            return View();
        }
    }
}