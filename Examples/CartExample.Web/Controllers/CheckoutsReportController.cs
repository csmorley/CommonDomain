using CartExample.Projections;
using CommonDomain.Messaging;
using Newtonsoft.Json;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CartExample.Web.Controllers
{
    public class CheckoutsReportController : Controller
    {
        public CheckoutsReportController(Database readModel)
        {
            this.readModel = readModel;
        }

        Database readModel;

        // GET: CheckoutsReport
        public ActionResult Index(int month = 1)
        {
            if (month < 1 || month > 12)
                month = 1;

            var monthData = readModel.CheckoutsByDate.Where(x => x.Key.Month == month).ToList();

            List<object> data = new List<object>();
            data.Add(new[] { "Day", "Total", "Missed Opportunities", "%", "Tweets" });
            ulong total = 0;
            foreach(var day in monthData)
            {
                ulong tweetsForDay = 0;
                this.readModel.TweetsByDate.TryGetValue(day.Value.Day.Date, out tweetsForDay);

                data.Add(new object[] {
                    day.Value.Day.Date.DayOfWeek.ToString(),
                    day.Value.TotalCarts,
                    day.Value.CartsThatHadAnAbandonedItem,
                    day.Value.PercentageWithAbandonedItems,
                    tweetsForDay
                });
                total = total + day.Value.TotalCarts;
            }

            ViewBag.TotalCheckoutsForMonth = total;
            ViewBag.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            ViewBag.Data = JsonConvert.SerializeObject(data);

            return View();
        }

        public ActionResult Year(int year)
        {
            var yearData = readModel.CheckoutsByDate.Where(x => x.Key.Year == year).ToList();

            List<object> data = new List<object>();
            data.Add(new[] { "Day", "Total", "Missed Opportunities", "%", "Tweets" });
            ulong total = 0;
            foreach (var day in yearData)
            {
                ulong tweetsForDay = 0;
                this.readModel.TweetsByDate.TryGetValue(day.Value.Day.Date, out tweetsForDay);

                data.Add(new object[] {
                    day.Value.Day.Date.DayOfWeek.ToString(),
                    day.Value.TotalCarts,
                    day.Value.CartsThatHadAnAbandonedItem,
                    day.Value.PercentageWithAbandonedItems,
                    tweetsForDay
                });
                total = total + day.Value.TotalCarts;
            }

            ViewBag.TotalCheckoutsForMonth = total;
            ViewBag.Period = year;
            ViewBag.Data = JsonConvert.SerializeObject(data);

            return View("Index");
        }
    }
}