using Swimduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swimduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Opis aplikacji";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Chętnie odpowiem na wszelkie pytania oraz uwagi odnośnie aplikacji.";

            return View();
        }      
    }
}