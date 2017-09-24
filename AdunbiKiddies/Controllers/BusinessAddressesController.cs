using AdunbiKiddies.Models;
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
            ViewBag.MerchantId = new SelectList(_db.Merchants.AsNoTracking()
                        .Where(x => x.MerchantId.Equals(merchantId)), "MerchantId", "FullName");
            return View();
        }

        // POST: BusinessAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessAddressId,MerchantId,AddressType,BuildingNo,StreetName,TownName,StateName")] BusinessAddress businessAddress)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessAddresses.Add(businessAddress);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", businessAddress.MerchantId);
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
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "FullName", businessAddress.MerchantId);
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
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", businessAddress.MerchantId);
            return View(businessAddress);
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
