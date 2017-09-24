using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class MerchantsController : BaseController
    {
        // private AjaoOko_db _db = new AjaoOko_db();

        // GET: MerchantsTest
        public async Task<ActionResult> Index()
        {
            var merchants = _db.Merchants.Include(m => m.PartnerShipAgreement);
            return View(await merchants.ToListAsync());
        }

        // GET: MerchantsTest/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Merchant merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        // GET: MerchantsTest/Create
        public ActionResult Create()
        {
            ViewBag.MerchantId = new SelectList(_db.PartnerShipAgreements, "MerchantId", "WebisteLink");
            return View();
        }

        // POST: MerchantsTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MerchantId,CompanyName,PreferedStoreName,IsReffered,BusinessEntry,IdCardName,RegistrationNo,IsVerified,FirstName,MiddleName,LastName,Gender,Email,PhoneNumber,TownOfBirth,StateOfOrigin,Nationality,Passport")] Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                _db.Merchants.Add(merchant);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MerchantId = new SelectList(_db.PartnerShipAgreements, "MerchantId", "WebisteLink", merchant.MerchantId);
            return View(merchant);
        }

        // GET: MerchantsTest/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Merchant merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            ViewBag.MerchantId = new SelectList(_db.PartnerShipAgreements, "MerchantId", "WebisteLink", merchant.MerchantId);
            return View(merchant);
        }

        // POST: MerchantsTest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MerchantId,CompanyName,PreferedStoreName,IsReffered,BusinessEntry,IdCardName,RegistrationNo,IsVerified,FirstName,MiddleName,LastName,Gender,Email,PhoneNumber,TownOfBirth,StateOfOrigin,Nationality,Passport")] Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(merchant).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MerchantId = new SelectList(_db.PartnerShipAgreements, "MerchantId", "WebisteLink", merchant.MerchantId);
            return View(merchant);
        }

        // GET: MerchantsTest/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Merchant merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        // POST: MerchantsTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Merchant merchant = await _db.Merchants.FindAsync(id);
            _db.Merchants.Remove(merchant);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(string id)
        {
            var item = await _db.Merchants.FindAsync(id);

            byte[] photoBack = item.Passport;

            return File(photoBack, "image/png");
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
