using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineKladilnica.Models;
using Microsoft.AspNet.Identity;

namespace OnlineKladilnica.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UserIndex()
        {
            string userId = GetLoggedInUserId();
            return View(await db.Tickets.Where(x => x.UserFk == userId).Include(x => x.TiketUtakmicas).ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminIndex()
        {
            return View(await db.Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Created,Platena")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.UserFk = GetLoggedInUserId();
                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }

            return View(ticket);
        }

        // Get: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult> UserCreate()
        {
            Ticket ticket = new Ticket()
            {
                UserFk = GetLoggedInUserId(),
                Created = DateTime.Now,
                Platena = false
            };
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("UserIndex");
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Created,Platena")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.UserFk = GetLoggedInUserId();
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin, User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("UserIndex");
        }

        protected string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
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
