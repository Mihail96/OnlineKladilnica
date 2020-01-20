using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using OnlineKladilnica.Models;

namespace OnlineKladilnica.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TimsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tims
        public async Task<ActionResult> AdminIndex()
        {
            return View(await db.Timovi.ToListAsync());
        }

        // GET: Tims
        [AllowAnonymous]
        public async Task<ActionResult> UserIndex()
        {
            return View(await db.Timovi.ToListAsync());
        }

        // GET: Tims/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tim tim = await db.Timovi.FindAsync(id);
            if (tim == null)
            {
                return HttpNotFound();
            }
            return View(tim);
        }

        // GET: Tims/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Ime")] Tim tim)
        {
            if (ModelState.IsValid)
            {
                db.Timovi.Add(tim);
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }

            return View(tim);
        }

        // GET: Tims/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tim tim = await db.Timovi.FindAsync(id);
            if (tim == null)
            {
                return HttpNotFound();
            }
            return View(tim);
        }

        // POST: Tims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Ime")] Tim tim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tim).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }
            return View(tim);
        }

        // GET: Tims/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tim tim = await db.Timovi.FindAsync(id);
            if (tim == null)
            {
                return HttpNotFound();
            }
            return View(tim);
        }

        // POST: Tims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tim tim = await db.Timovi.FindAsync(id);
            db.Timovi.Remove(tim);
            await db.SaveChangesAsync();
            return RedirectToAction("UserIndex");
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
