using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Swimduler.Models;

namespace Swimduler.Controllers
{
    public class LessonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Group);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        // POST: Lessons/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Beginning,Duration,Cycle,CycleEnd,ThemeColor,GroupId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();

                var newEvent = CreateEventFromLesson(lesson);

                if (lesson.Cycle == Lesson.LessonCycle.Brak)
                {
                    if (new HomeController().AddEventToDatabase(newEvent))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var newEvents = FindOccurrences(newEvent, lesson.Cycle, lesson.CycleEnd);
                    if (new HomeController().AddEventsToDatabase(newEvents))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Beginning,Duration,Cycle,CycleEnd,ThemeColor,GroupId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                var referencedEvents = db.CalendarEvents.Where(x => x.LessonId == lesson.Id).ToList();
                db.CalendarEvents.RemoveRange(referencedEvents);
                db.SaveChanges();

                var newEvent = CreateEventFromLesson(lesson);

                if (lesson.Cycle == Lesson.LessonCycle.Brak)
                {
                    if (new HomeController().AddEventToDatabase(newEvent))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var newEvents = FindOccurrences(newEvent, lesson.Cycle, lesson.CycleEnd);
                    if (new HomeController().AddEventsToDatabase(newEvents))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);

            var referencedEvents = db.CalendarEvents.Where(x => x.LessonId == lesson.Id).ToList();
            db.CalendarEvents.RemoveRange(referencedEvents);

            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [NonAction]
        private CalendarEvent CreateEventFromLesson(Lesson lesson)
        {
            var group = db.Groups.Include("Client_Groups").FirstOrDefault(l => l.Id == lesson.GroupId);

            var newEvent = new CalendarEvent
            {
                Subject = group.Name,
                Comments = "czas trwania: " + lesson.Duration.Hours + "h " + lesson.Duration.Minutes + "min, \n" +
                    "cykl lekcji: " + lesson.Cycle + ", \n" +
                    "wielkość grupy: " + group.Client_Groups.Count,
                Start = lesson.Beginning,
                End = lesson.Beginning + lesson.Duration,
                ThemeColor = lesson.ThemeColor,
                LessonId = lesson.Id
            };

            return newEvent;
        }

        [NonAction]
        private List<CalendarEvent> FindOccurrences(CalendarEvent firstEvent, Lesson.LessonCycle cycle, DateTime? endDate)
        {
            if (endDate == null)
            {
                return null;
            }

            switch (cycle)
            {
                case Lesson.LessonCycle.Tygodniowy:
                    return GetEventsWithinCycle(7, firstEvent, endDate);
                case Lesson.LessonCycle.Dwutygodniowy:
                    return GetEventsWithinCycle(14, firstEvent, endDate);
                case Lesson.LessonCycle.Miesięczny:
                    return GetEventsWithinCycle(28, firstEvent, endDate);
            }

            return null;
        }

        [NonAction]
        private List<CalendarEvent> GetEventsWithinCycle(double cycleDays, CalendarEvent firstEvent, DateTime? endDate)
        {
            var result = new List<CalendarEvent>() { firstEvent };

            if (endDate == null)
            {
                return result;
            }

            var currentStart = firstEvent.Start;
            var currentEnd = firstEvent.End;

            while ((currentStart = currentStart.AddDays(cycleDays)).Date <= endDate.Value.Date)
            {
                currentEnd = currentEnd.AddDays(cycleDays);

                var newEvent = new CalendarEvent
                {
                    Subject = firstEvent.Subject,
                    Comments = firstEvent.Comments,
                    Start = currentStart,
                    End = currentEnd,
                    ThemeColor = firstEvent.ThemeColor,
                    LessonId = firstEvent.LessonId
                };

                result.Add(newEvent);
            }

            return result;
        }

    }
}
