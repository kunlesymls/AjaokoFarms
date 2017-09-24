using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class BusinessDocumentsController : BaseController
    {
        // GET: BusinessDocuments
        public async Task<ActionResult> Index()
        {
            var businessDocuments = _db.BusinessDocuments.Include(b => b.BusinessRegistration);
            return View(await businessDocuments.ToListAsync());
        }

        // GET: BusinessDocuments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessDocument businessDocument = await _db.BusinessDocuments.FindAsync(id);
            if (businessDocument == null)
            {
                return HttpNotFound();
            }
            return View(businessDocument);
        }

        // GET: BusinessDocuments/Create
        public ActionResult Create()
        {
            ViewBag.BusinessRegistrationId = new SelectList(_db.BusinessRegistrations, "BusinessRegistrationId", "MerchantId");
            return View();
        }

        // POST: BusinessDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessDocumentId,BusinessRegistrationId,Document")] BusinessDocument businessDocument)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessDocuments.Add(businessDocument);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessRegistrationId = new SelectList(_db.BusinessRegistrations, "BusinessRegistrationId", "MerchantId", businessDocument.BusinessRegistrationId);
            return View(businessDocument);
        }

        // GET: BusinessDocuments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessDocument businessDocument = await _db.BusinessDocuments.FindAsync(id);
            if (businessDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessRegistrationId = new SelectList(_db.BusinessRegistrations, "BusinessRegistrationId", "MerchantId", businessDocument.BusinessRegistrationId);
            return View(businessDocument);
        }

        // POST: BusinessDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessDocumentId,BusinessRegistrationId,Document")] BusinessDocument businessDocument)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(businessDocument).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessRegistrationId = new SelectList(_db.BusinessRegistrations, "BusinessRegistrationId", "MerchantId", businessDocument.BusinessRegistrationId);
            return View(businessDocument);
        }

        // GET: BusinessDocuments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessDocument businessDocument = await _db.BusinessDocuments.FindAsync(id);
            if (businessDocument == null)
            {
                return HttpNotFound();
            }
            return View(businessDocument);
        }

        // POST: BusinessDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BusinessDocument businessDocument = await _db.BusinessDocuments.FindAsync(id);
            _db.BusinessDocuments.Remove(businessDocument);
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
