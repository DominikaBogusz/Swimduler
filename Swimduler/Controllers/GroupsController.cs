using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Swimduler.Models;
using Swimduler.Models.Views;

namespace Swimduler.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            var results = from c in db.Clients
                          select new
                          {
                              c.Id,
                              c.FirstName,
                              c.SecondName,
                              Checked = ((from cg in db.Client_Groups
                                          where (cg.GroupId == id) && (cg.ClientId == c.Id)
                                          select cg).Count() > 0)
                          };

            var viewModel = new GroupsViewModel();

            viewModel.Id = id.Value;
            viewModel.Name = group.Name;

            var checkBoxList = new List<CheckBoxViewModel>();
            foreach (var item in results)
            {
                checkBoxList.Add(new CheckBoxViewModel { Id = item.Id, Name = item.FirstName + " " + item.SecondName, Checked = item.Checked });
            }

            viewModel.Clients = checkBoxList;

            return View(viewModel);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            var results = from c in db.Clients
                          select new
                          {
                              c.Id,
                              c.FirstName,
                              c.SecondName,
                              Checked = ((from cg in db.Client_Groups
                                          where (cg.GroupId == id) && (cg.ClientId == c.Id)
                                          select cg).Count() > 0)
                          };

            var viewModel = new GroupsViewModel();

            viewModel.Id = id.Value;
            viewModel.Name = group.Name;

            var checkBoxList = new List<CheckBoxViewModel>();
            foreach(var item in results)
            {
                checkBoxList.Add(new CheckBoxViewModel { Id = item.Id, Name = item.FirstName + " " + item.SecondName, Checked = item.Checked });
            }

            viewModel.Clients = checkBoxList;

            return View(viewModel);
        }

        // POST: Groups/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupsViewModel group)
        {
            if (ModelState.IsValid)
            {
                var reqGroup = db.Groups.Find(group.Id);

                reqGroup.Name = group.Name;

                foreach (var cg in db.Client_Groups)
                {
                    if(cg.GroupId == group.Id)
                    {
                        db.Entry(cg).State = EntityState.Deleted;
                    }
                }

                foreach (var c in group.Clients)
                {
                    if (c.Checked)
                    {
                        db.Client_Groups.Add(new Client_Group { ClientId = c.Id, GroupId = group.Id });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);

            foreach (var cg in db.Client_Groups)
            {
                if (cg.GroupId == group.Id)
                {
                    db.Entry(cg).State = EntityState.Deleted;
                }
            }

            var relatedLessons = db.Lessons.Include("CalendarEvents").Where(l => l.GroupId == group.Id);

            foreach (var l in relatedLessons)
            {
                var referencedEvents = l.CalendarEvents.Where(x => x.LessonId == l.Id);
                db.CalendarEvents.RemoveRange(referencedEvents);

                db.Lessons.Remove(l);
            }

            db.Groups.Remove(group);
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
    }
}
