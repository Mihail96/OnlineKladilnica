namespace OnlineKladilnica.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using OnlineKladilnica.Models;
    using System.Linq;
    using System;

    public class UtakmicasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public async Task<ActionResult> UserIndex()
        {
            var utakmici = db.Utakmici.Where(x => x.Vreme > DateTime.Now).Include(u => u.ATim).Include(u => u.BTim);
            return View(await utakmici.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminIndex()
        {
            var utakmici = db.Utakmici.Where(x => x.Vreme > DateTime.Now).Include(u => u.ATim).Include(u => u.BTim);
            return View(await utakmici.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Utakmicas/Create
        public ActionResult Create()
        {
            ViewBag.ATimeFk = new SelectList(db.Timovi, "Id", "Ime");
            ViewBag.BTimeFk = new SelectList(db.Timovi, "Id", "Ime");
            return View();
        }

        // POST: Utakmicas/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ImeUtakmica,Vreme,CoefA,CoefB,ATimeFk,BTimeFk")] Utakmica utakmica)
        {
            if (ModelState.IsValid)
            {
                db.Utakmici.Add(utakmica);
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }

            ViewBag.ATimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.ATimeFk);
            ViewBag.BTimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.BTimeFk);
            return View(utakmica);
        }

        // GET: Utakmicas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utakmica utakmica = await db.Utakmici.FindAsync(id);
            if (utakmica == null)
            {
                return HttpNotFound();
            }
            ViewBag.ATimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.ATimeFk);
            ViewBag.BTimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.BTimeFk);
            return View(utakmica);
        }

        // POST: Utakmicas/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ImeUtakmica,Vreme,CoefA,CoefB,ATimeFk,BTimeFk")] Utakmica utakmica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utakmica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("UserIndex");
            }
            ViewBag.ATimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.ATimeFk);
            ViewBag.BTimeFk = new SelectList(db.Timovi, "Id", "Ime", utakmica.BTimeFk);
            return View(utakmica);
        }

        // GET: Utakmicas/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utakmica utakmica = await db.Utakmici.FindAsync(id);
            if (utakmica == null)
            {
                return HttpNotFound();
            }
            return View(utakmica);
        }

        // POST: Utakmicas/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Utakmica utakmica = await db.Utakmici.FindAsync(id);
            db.Utakmici.Remove(utakmica);
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
