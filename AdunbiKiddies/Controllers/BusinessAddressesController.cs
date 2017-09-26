using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class BusinessAddressesController : BaseController
    {

        // GET: BusinessAddresses
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                var userId = User.Identity.GetUserId();
                var bAddress = _db.BusinessAddresses.Include(b => b.Merchant).AsNoTracking()
                    .Where(x => x.MerchantId.Equals(userId));
                return View(await bAddress.ToListAsync());

            }
            var businessAddresses = _db.BusinessAddresses.Include(b => b.Merchant);
            return View(await businessAddresses.ToListAsync());
        }

        // GET: BusinessAddresses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAddress businessAddress = await _db.BusinessAddresses.FindAsync(id);
            if (businessAddress == null)
            {
                return HttpNotFound();
            }
            return View(businessAddress);
        }

        // GET: BusinessAddresses/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName");
            return View();
        }

        // POST: BusinessAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BusinessAddress businessAddress)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessAddresses.Add(businessAddress);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessAddress.MerchantId);
            return View(businessAddress);
        }

        // GET: BusinessAddresses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAddress businessAddress = await _db.BusinessAddresses.FindAsync(id);
            if (businessAddress == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessAddress.MerchantId);
            return View(businessAddress);
        }

        // POST: BusinessAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessAddressId,MerchantId,AddressType,BuildingNo,StreetName,TownName,StateName")] BusinessAddress businessAddress)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(businessAddress).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var userId = User.Identity.GetUserId();
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                .Where(x => merchantId.Equals(userId)), "MerchantId", "FullName", businessAddress.MerchantId); return View(businessAddress);
        }

        // GET: BusinessAddresses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAddress businessAddress = await _db.BusinessAddresses.FindAsync(id);
            if (businessAddress == null)
            {
                return HttpNotFound();
            }
            return View(businessAddress);
        }

        // POST: BusinessAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BusinessAddress businessAddress = await _db.BusinessAddresses.FindAsync(id);
            _db.BusinessAddresses.Remove(businessAddress);
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
