using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class ShippingDetailsController : Controller
    {
        private AjaoOkoDb db = new AjaoOkoDb();

        // GET: ShippingDetails
        public async Task<ActionResult> Index()
        {
            var shippingDetails = db.ShippingDetails.Include(s => s.Customer);
            return View(await shippingDetails.ToListAsync());
        }

        // GET: ShippingDetails/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingDetail shippingDetail = await db.ShippingDetails.FindAsync(id);
            if (shippingDetail == null)
            {
                return HttpNotFound();
            }
            return View(shippingDetail);
        }

        // GET: ShippingDetails/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            return View();
        }

        // POST: ShippingDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId,AddressType,BuildingNo,StreetName,TownName,StateName")] ShippingDetail shippingDetail)
        {
            if (ModelState.IsValid)
            {
                db.ShippingDetails.Add(shippingDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", shippingDetail.CustomerId);
            return View(shippingDetail);
        }

        // GET: ShippingDetails/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingDetail shippingDetail = await db.ShippingDetails.FindAsync(id);
            if (shippingDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", shippingDetail.CustomerId);
            return View(shippingDetail);
        }

        // POST: ShippingDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerId,AddressType,BuildingNo,StreetName,TownName,StateName")] ShippingDetail shippingDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", shippingDetail.CustomerId);
            return View(shippingDetail);
        }

        // GET: ShippingDetails/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingDetail shippingDetail = await db.ShippingDetails.FindAsync(id);
            if (shippingDetail == null)
            {
                return HttpNotFound();
            }
            return View(shippingDetail);
        }

        // POST: ShippingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ShippingDetail shippingDetail = await db.ShippingDetails.FindAsync(id);
            db.ShippingDetails.Remove(shippingDetail);
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
