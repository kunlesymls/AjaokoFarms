using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class StoreSectionsController : BaseController
    {
        // GET: StoreSections
        public async Task<ActionResult> Index()
        {
            return View(await _db.StoreSections.ToListAsync());
        }

        // GET: StoreSections/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreSection storeSection = await _db.StoreSections.FindAsync(id);
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
                _db.StoreSections.Add(storeSection);
                await _db.SaveChangesAsync();
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
            StoreSection storeSection = await _db.StoreSections.FindAsync(id);
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
                _db.Entry(storeSection).State = EntityState.Modified;
                await _db.SaveChangesAsync();
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
            StoreSection storeSection = await _db.StoreSections.FindAsync(id);
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
            StoreSection storeSection = await _db.StoreSections.FindAsync(id);
            _db.StoreSections.Remove(storeSection);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
