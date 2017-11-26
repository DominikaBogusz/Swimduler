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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var events = db.CalendarEvents.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(CalendarEvent calendarEvent)
        {
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (calendarEvent.Id > 0)
                {
                    var reqEvent = db.CalendarEvents.Where(x => x.Id == calendarEvent.Id).FirstOrDefault();
                    if (reqEvent != null)
                    {
                        reqEvent.Subject = calendarEvent.Subject;
                        reqEvent.Start = calendarEvent.Start;
                        reqEvent.End = calendarEvent.End;
                        reqEvent.Comments = calendarEvent.Comments;
                        reqEvent.ThemeColor = calendarEvent.ThemeColor;
                    }
                }
                else
                {
                    db.CalendarEvents.Add(calendarEvent);
                }
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var reqEvent = db.CalendarEvents.Where(x => x.Id == eventID).FirstOrDefault();
                if (reqEvent != null)
                {
                    db.CalendarEvents.Remove(reqEvent);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}