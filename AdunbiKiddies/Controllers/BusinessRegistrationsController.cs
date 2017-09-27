using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
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
            //if (Request.IsAuthenticated && User.IsInRole("Admin"))
            //{
            //    var userId = User.Identity.GetUserId();
            //    var bReg = _db.BusinessRegistrations.Include(b => b.Merchant).AsNoTracking()
            //                                    .Where(x => x.MerchantId.Equals(userId));
            //    return View(await bReg.ToListAsync());

            //}
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
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                                    .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName");
            return View();
        }

        // POST: BusinessRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BusinessRegistration businessRegistration)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessRegistrations.Add(businessRegistration);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessRegistration.MerchantId);
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
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessRegistration.MerchantId); return View(businessRegistration);
        }

        // POST: BusinessRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BusinessRegistration businessRegistration)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(businessRegistration).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessRegistration.MerchantId);
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
