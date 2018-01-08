using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swimduler.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomError(int id)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = id;

            switch (id)
            {
                case 400:
                    ViewBag.Message = "Żądanie nie może być zrealizowne. Sprawdź poprawność wprowadzonego adresu.";
                    break;
                case 404:
                    ViewBag.Message = "Podany adres nie istnieje. Sprawdź, czy odwołujesz się do prawidłowego zasobu serwera.";
                    break;
                case 500:
                    ViewBag.Message = "Nastąpił wewnętrzny błąd serwera. Nasi eksperci już nad nim pracują.";
                    break;
            }

            return View();
        }
    }
}