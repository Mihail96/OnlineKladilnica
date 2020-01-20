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
    public class TiketUtakmicasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TiketUtakmicas
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UserIndex()
        {
            string userId = GetLoggedInUserId();
            var tiketUtakmici = db.TiketUtakmici.Include(t => t.Ticket).Where(x => x.Ticket.UserFk == userId).Include(t => t.Utakmica);
            return View(await tiketUtakmici.ToListAsync());
        }

        // GET: TiketUtakmicas
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminIndex()
        {
            var tiketUtakmici = db.TiketUtakmici.Include(t => t.Ticket).Include(t => t.Utakmica);
            return View(await tiketUtakmici.ToListAsync());
        }

        // GET: TiketUtakmicas/Details/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiketUtakmica tiketUtakmica = await db.TiketUtakmici.FindAsync(id);
            if (tiketUtakmica == null)
            {
                return HttpNotFound();
            }
            return View(tiketUtakmica);
        }

        // GET: TiketUtakmicas/Create
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            ViewBag.TicketFk = new SelectList(db.Tickets, "Id", "Created");
            ViewBag.UtakmicaFk = new SelectList(db.Utakmici, "Id", "ImeUtakmica");
            return View();
        }

        // POST: TiketUtakmicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TimAResult,TimBResult,Oblog,TicketFk,UtakmicaFk")] TiketUtakmica tiketUtakmica)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = await db.Tickets.Where(x => x.Id == tiketUtakmica.TicketFk).SingleOrDefaultAsync();
                Utakmica utakmica = await db.Utakmici.Where(x => x.Id == tiketUtakmica.UtakmicaFk).SingleOrDefaultAsync();
                decimal koeficient = 1;
                if(tiketUtakmica.TimAResult > tiketUtakmica.TimBResult)
                {
                    koeficient = utakmica.CoefA;
                }
                else if(tiketUtakmica.TimAResult < tiketUtakmica.TimBResult)
                {
                    koeficient = utakmica.CoefB;
                }
                else
                {
                    koeficient = (utakmica.CoefA + utakmica.CoefB)*0.8M;
                }
                tiketUtakmica.Zarabotka = tiketUtakmica.Oblog * koeficient;
                db.TiketUtakmici.Add(tiketUtakmica);
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }

            ViewBag.TicketFk = new SelectList(db.Tickets, "Id", "UserFk", tiketUtakmica.TicketFk);
            ViewBag.UtakmicaFk = new SelectList(db.Utakmici, "Id", "ImeUtakmica", tiketUtakmica.UtakmicaFk);
            return View(tiketUtakmica);
        }

        // GET: TiketUtakmicas/Edit/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiketUtakmica tiketUtakmica = await db.TiketUtakmici.FindAsync(id);
            if (tiketUtakmica == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketFk = new SelectList(db.Tickets, "Id", "Created", tiketUtakmica.TicketFk);
            ViewBag.UtakmicaFk = new SelectList(db.Utakmici, "Id", "ImeUtakmica", tiketUtakmica.UtakmicaFk);
            return View(tiketUtakmica);
        }

        // POST: TiketUtakmicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TimAResult,TimBResult,Oblog,TicketFk,UtakmicaFk")] TiketUtakmica tiketUtakmica)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = await db.Tickets.Where(x => x.Id == tiketUtakmica.TicketFk).SingleOrDefaultAsync();
                Utakmica utakmica = await db.Utakmici.Where(x => x.Id == tiketUtakmica.UtakmicaFk).SingleOrDefaultAsync();
                decimal koeficient = 1;
                if (tiketUtakmica.TimAResult > tiketUtakmica.TimBResult)
                {
                    koeficient = utakmica.CoefA;
                }
                else if (tiketUtakmica.TimAResult < tiketUtakmica.TimBResult)
                {
                    koeficient = utakmica.CoefB;
                }
                else
                {
                    koeficient = (utakmica.CoefA + utakmica.CoefB) * 0.8M;
                }
                tiketUtakmica.Zarabotka = tiketUtakmica.Oblog * koeficient;
                db.Entry(tiketUtakmica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }
            ViewBag.TicketFk = new SelectList(db.Tickets, "Id", "UserFk", tiketUtakmica.TicketFk);
            ViewBag.UtakmicaFk = new SelectList(db.Utakmici, "Id", "ImeUtakmica", tiketUtakmica.UtakmicaFk);
            return View(tiketUtakmica);
        }

        // GET: TiketUtakmicas/Delete/5
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiketUtakmica tiketUtakmica = await db.TiketUtakmici.FindAsync(id);
            if (tiketUtakmica == null)
            {
                return HttpNotFound();
            }
            return View(tiketUtakmica);
        }

        // POST: TiketUtakmicas/Delete/5
        [Authorize(Roles = "Admin, User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TiketUtakmica tiketUtakmica = await db.TiketUtakmici.FindAsync(id);
            db.TiketUtakmici.Remove(tiketUtakmica);
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

        protected string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }
    }
}
