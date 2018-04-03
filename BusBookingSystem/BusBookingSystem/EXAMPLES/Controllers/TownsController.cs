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
    [Authorize(Roles = "Admin")]
    public class TownsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Towns
        public async Task<ActionResult> Index()
        {
            return View(await db.Towns.ToListAsync());
        }

        // GET: Towns/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = await db.Towns.FindAsync(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }

        // GET: Towns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Towns/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TownId,Name")] Town town)
        {
            if (ModelState.IsValid)
            {
                db.Towns.Add(town);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(town);
        }

        // GET: Towns/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = await db.Towns.FindAsync(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }

        // POST: Towns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TownId,Name")] Town town)
        {
            if (ModelState.IsValid)
            {
                db.Entry(town).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(town);
        }

        // GET: Towns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = await db.Towns.FindAsync(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }

        // POST: Towns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Town town = await db.Towns.FindAsync(id);
            db.Towns.Remove(town);
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
