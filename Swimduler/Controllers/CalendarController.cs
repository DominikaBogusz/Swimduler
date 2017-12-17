using Swimduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swimduler.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var events = db.CalendarEvents
                        .Select(e => new
                        {
                            Id = e.Id,
                            Subject = e.Subject,
                            Comments = e.Comments,
                            Start = e.Start,
                            End = e.End,
                            ThemeColor = e.ThemeColor
                        }).ToList();

                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(CalendarEvent calendarEvent)
        {
            bool status;

            if (calendarEvent.Id > 0)
            {
                status = EditEventInDatabase(calendarEvent);
            }
            else
            {
                status = AddEventToDatabase(calendarEvent);
            }

            return new JsonResult { Data = new { status = status } };
        }

        [NonAction]
        public bool AddEventToDatabase(CalendarEvent calendarEvent)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.CalendarEvents.Add(calendarEvent);
                db.SaveChanges();
                return true;
            }
        }

        [NonAction]
        public bool AddEventsToDatabase(List<CalendarEvent> calendarEvents)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.CalendarEvents.AddRange(calendarEvents);
                db.SaveChanges();
                return true;
            }
        }

        [NonAction]
        public bool EditEventInDatabase(CalendarEvent calendarEvent)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var reqEvent = db.CalendarEvents.Where(x => x.Id == calendarEvent.Id).FirstOrDefault();
                if (reqEvent != null)
                {
                    reqEvent.Subject = calendarEvent.Subject;
                    reqEvent.Start = calendarEvent.Start;
                    reqEvent.End = calendarEvent.End;
                    reqEvent.Comments = calendarEvent.Comments;
                    reqEvent.ThemeColor = calendarEvent.ThemeColor;
                    reqEvent.LessonId = reqEvent.LessonId;
                }
                db.SaveChanges();
                return true;
            }
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