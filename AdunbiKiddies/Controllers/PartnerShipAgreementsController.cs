using Microsoft.AspNet.Identity;
using OpenOrderFramework.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class PartnerShipAgreementsController : BaseController
    {

        // GET: PartnerShipAgreements
        public async Task<ActionResult> Index()
        {
            var partnerShipAgreements = _db.PartnerShipAgreements.Include(p => p.Category).Include(p => p.Merchant);
            if (User.IsInRole("Admin"))
            {
                return View(await partnerShipAgreements.Where(x => x.MerchantId.Equals(merchantId)).ToListAsync());
            }
            return View(await partnerShipAgreements.ToListAsync());
        }

        // GET: PartnerShipAgreements/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerShipAgreement partnerShipAgreement = await _db.PartnerShipAgreements.FindAsync(id);
            if (partnerShipAgreement == null)
            {
                return HttpNotFound();
            }
            return View(partnerShipAgreement);
        }

        // GET: PartnerShipAgreements/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName");
            return View();
        }

        // POST: PartnerShipAgreements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PartnerShipAgreement partnerShipAgreement)
        {
            if (ModelState.IsValid)
            {
                _db.PartnerShipAgreements.Add(partnerShipAgreement);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", partnerShipAgreement.CategoryId);
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", partnerShipAgreement.MerchantId);
            return View(partnerShipAgreement);
        }

        // GET: PartnerShipAgreements/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerShipAgreement partnerShipAgreement = await _db.PartnerShipAgreements.FindAsync(id);
            if (partnerShipAgreement == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", partnerShipAgreement.CategoryId);
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", partnerShipAgreement.MerchantId);
            return View(partnerShipAgreement);
        }

        // POST: PartnerShipAgreements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PartnerShipAgreement partnerShipAgreement)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(partnerShipAgreement).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", partnerShipAgreement.CategoryId);
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", partnerShipAgreement.MerchantId);
            return View(partnerShipAgreement);
        }

        // GET: PartnerShipAgreements/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerShipAgreement partnerShipAgreement = await _db.PartnerShipAgreements.FindAsync(id);
            if (partnerShipAgreement == null)
            {
                return HttpNotFound();
            }
            return View(partnerShipAgreement);
        }

        // POST: PartnerShipAgreements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            PartnerShipAgreement partnerShipAgreement = await _db.PartnerShipAgreements.FindAsync(id);
            _db.PartnerShipAgreements.Remove(partnerShipAgreement);
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
