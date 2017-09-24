using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class BusinessRegistrationsController : BaseController
    {
        // GET: BusinessRegistrations
        public async Task<ActionResult> Index()
        {
            var businessRegistrations = _db.BusinessRegistrations.Include(b => b.Merchant);
            return View(await businessRegistrations.ToListAsync());
        }

        // GET: BusinessRegistrations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessRegistration businessRegistration = await _db.BusinessRegistrations.FindAsync(id);
            if (businessRegistration == null)
            {
                return HttpNotFound();
            }
            return View(businessRegistration);
        }

        // GET: BusinessRegistrations/Create
        public ActionResult Create()
        {
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName");
            return View();
        }

        // POST: BusinessRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessRegistrationId,MerchantId,RegistrationNo,LegalName")] BusinessRegistration businessRegistration)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessRegistrations.Add(businessRegistration);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", businessRegistration.MerchantId);
            return View(businessRegistration);
        }

        // GET: BusinessRegistrations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessRegistration businessRegistration = await _db.BusinessRegistrations.FindAsync(id);
            if (businessRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", businessRegistration.MerchantId);
            return View(businessRegistration);
        }

        // POST: BusinessRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessRegistrationId,MerchantId,RegistrationNo,LegalName")] BusinessRegistration businessRegistration)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(businessRegistration).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", businessRegistration.MerchantId);
            return View(businessRegistration);
        }

        // GET: BusinessRegistrations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessRegistration businessRegistration = await _db.BusinessRegistrations.FindAsync(id);
            if (businessRegistration == null)
            {
                return HttpNotFound();
            }
            return View(businessRegistration);
        }

        // POST: BusinessRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BusinessRegistration businessRegistration = await _db.BusinessRegistrations.FindAsync(id);
            _db.BusinessRegistrations.Remove(businessRegistration);
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
