using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdunbiKiddies.Models;

namespace AdunbiKiddies.Controllers
{
    public class StoreSectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StoreSections
        public async Task<ActionResult> Index()
        {
            return View(await db.StoreSections.ToListAsync());
        }

        // GET: StoreSections/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreSection storeSection = await db.StoreSections.FindAsync(id);
            if (storeSection == null)
            {
                return HttpNotFound();
            }
            return View(storeSection);
        }

        // GET: StoreSections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StoreSectionId,SectionName")] StoreSection storeSection)
        {
            if (ModelState.IsValid)
            {
                db.StoreSections.Add(storeSection);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(storeSection);
        }

        // GET: StoreSections/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreSection storeSection = await db.StoreSections.FindAsync(id);
            if (storeSection == null)
            {
                return HttpNotFound();
            }
            return View(storeSection);
        }

        // POST: StoreSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StoreSectionId,SectionName")] StoreSection storeSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeSection).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(storeSection);
        }

        // GET: StoreSections/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreSection storeSection = await db.StoreSections.FindAsync(id);
            if (storeSection == null)
            {
                return HttpNotFound();
            }
            return View(storeSection);
        }

        // POST: StoreSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StoreSection storeSection = await db.StoreSections.FindAsync(id);
            db.StoreSections.Remove(storeSection);
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
