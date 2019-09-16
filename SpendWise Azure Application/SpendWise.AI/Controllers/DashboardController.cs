using SpendWise.AI.Models;
using SpendWise.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ApplicationDashboardMVC.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewWindow(string url)
        {
            return Redirect(url);
        }

        public ActionResult Dashboard()
        {
            var predictiveSavings = SpendWiseServiceUtil.GetPredictiveSavings();
            ViewBag.SpendWiseExpense = "$" + predictiveSavings.Replace('"', ' ');
            ViewBag.Notifications = "5";
            return View();           
        }

        public ActionResult GetDailySuggestions(string productCategory)
        {
            List<SuggestionsViewModel> result = SpendWiseServiceUtil.GetSuggestions(productCategory);

            return PartialView("~/Views/Dashboard/Suggestion.cshtml", result);
        }



        public ActionResult DailyPredictiveExpense()
        {
            return PartialView("~/Views/Dashboard/PredictiveItems.cshtml", SpendWiseServiceUtil.GetDailyPredictiveItems());
        }

        public ActionResult GetPredictiveForecast()
        {
            var forecast = SpendWiseServiceUtil.GetPredictiveForecast();
            var monthlyExpense = forecast.Sum(x => Convert.ToInt32(x.Spending)).ToString();
            Session.Add("MonthlyExpense", monthlyExpense);
            return Json(new { result = forecast }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryAnalysis()
        {
            List<CategoryAnalysis> analysis = new List<CategoryAnalysis>();

            analysis.Add(
                new CategoryAnalysis()
                {
                    Category = "Junk",
                    Count = 2
                }
                );



            analysis.Add(
            new CategoryAnalysis()
            {
                Category = "Coffee",
                Count = 1
            }
                );


            analysis.Add(
new CategoryAnalysis()
{
    Category = "Entertainment",
    Count = 1
}
    );





            return Json(new
            {
                result = analysis
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyExpense()
        {
            if (Session["MonthlyExpense"] != null)
            {
                return Content("$ " + Session["MonthlyExpense"].ToString());
            }
            else return null;
        }
    }
}