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
        public ActionResult Edit([Bind(Include = "Id,Beginning,Duration,Cycle,ThemeColor,GroupId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
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

        private List<CalendarEvent> FindOccurrences(CalendarEvent firstEvent, Lesson.LessonCycle cycle, DateTime? endDate)
        {
            var result = new List<CalendarEvent> { firstEvent };

            if (endDate == null)
            {
                return result;
            }

            var currentStart = firstEvent.Start;
            var currentEnd = firstEvent.End;

            switch (cycle)
            {
                case Lesson.LessonCycle.Tygodniowy:
                    while ((currentStart = currentStart.AddDays(7)) <= endDate)
                    {
                        currentEnd = currentEnd.AddDays(7);

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
                    break;
                case Lesson.LessonCycle.Dwutygodniowy:
                    while ((currentStart = currentStart.AddDays(14)) <= endDate)
                    {
                        currentEnd = currentEnd.AddDays(14);

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
                    break;
                case Lesson.LessonCycle.Miesięczny:
                    while ((currentStart = currentStart.AddDays(28)) <= endDate)
                    {
                        currentEnd = currentEnd.AddDays(28);

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
                    break;
            }

            return result;
        }
    }
}
