using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventCalendar.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var events = db.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                if (e.EventID > 0)
                {
                    //Update event
                    var updatedEvent = db.Events.Where(x => x.EventID == e.EventID).FirstOrDefault();
                    if (updatedEvent != null)
                    {
                        updatedEvent.Subject = e.Subject;
                        updatedEvent.Start = e.Start;
                        updatedEvent.End = e.End;
                        updatedEvent.Description = e.Description;
                        updatedEvent.IsFullDay = e.IsFullDay;
                        updatedEvent.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    db.Events.Add(e);
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

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var deletedEvent = db.Events.Where(x => x.EventID == eventID).FirstOrDefault();
                if (deletedEvent != null)
                {
                    db.Events.Remove(deletedEvent);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };

        }
    }
}