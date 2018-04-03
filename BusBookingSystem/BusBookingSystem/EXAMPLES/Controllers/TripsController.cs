using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusBookingSystem.Models;

namespace BusBookingSystem.Controllers
{
    public class TripsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trips
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var trips = db.Trips.Include(t => t.Town);
            return View(await trips.ToListAsync());
        }

        // GET: Trips/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Trips/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TownId = new SelectList(db.Towns, "TownId", "Name");
            ViewBag.BusId = new SelectList(db.Buses, "BusId", "RegNo");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "TripId,TripDate,TownId,Departure,Arrival,BusId,SeatsAvailable,Fare")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                trip.setSeats(trip.BusId);
                db.Trips.Add(trip);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TownId = new SelectList(db.Towns, "TownId", "Name", trip.TownId);
            ViewBag.BusId = new SelectList(db.Buses, "BusId", "RegNo", trip.BusId);
            return View(trip);
        }

        [Authorize]
        public ActionResult Book(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip != null)
            {
                return View(trip);
            }
            return HttpNotFound();
        }

        [HttpPost,ActionName("Book")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> CompleteBooking(int id)
        {
            Booking b = new Booking();
            Trip trip = db.Trips.Find(id);
            var r = await trip.MakeBooking(trip.TripId, User.Identity.Name);
            var book = db.Bookings.OrderByDescending(x => x.TripId).First(i => i.TripId == id && i.UserName == User.Identity.Name);
            if (r)
            {
                await b.ConfirmBooking(book, book.UserName);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", r.ToString());
            return View(trip);
        }

        // GET: Trips/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.TownId = new SelectList(db.Towns, "TownId", "Name", trip.TownId);
            ViewBag.BusId = new SelectList(db.Buses, "BusId", "RegNo", trip.BusId);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "TripId,TripDate,TownId,Departure,Arrival,BusId,SeatsAvailable,Fare")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TownId = new SelectList(db.Towns, "TownId", "Name", trip.TownId);
            ViewBag.BusId = new SelectList(db.Buses, "BusId", "RegNo", trip.BusId);
            return View(trip);
        }

        // GET: Trips/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Trip trip = await db.Trips.FindAsync(id);
            db.Trips.Remove(trip);
            await db.SaveChangesAsync();
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
