using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class CustomersController : BaseController
    {
        // GET: Customers
        public async Task<ActionResult> Index()
        {
            var customers = _db.Customers.Include(c => c.ShippingDetail);
            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Customer customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_db.ShippingDetails, "CustomerId", "AddressType");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId,FirstName,MiddleName,LastName,Gender,Email,PhoneNumber,TownOfBirth,StateOfOrigin,Nationality,Passport")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(_db.ShippingDetails, "CustomerId", "AddressType", customer.CustomerId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(_db.ShippingDetails, "CustomerId", "AddressType", customer.CustomerId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.AsNoTracking().Where(x => x.Id.Equals(customer.CustomerId))
                                    .FirstOrDefaultAsync();
                user.FirstName = customer.FirstName;
                user.MiddleName = customer.MiddleName;
                user.LastName = customer.LastName;
                _db.Entry(user).State = EntityState.Modified;
                _db.Entry(customer).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Details");
            }
            ViewBag.CustomerId = new SelectList(_db.ShippingDetails, "CustomerId", "AddressType", customer.CustomerId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Customer customer = await _db.Customers.FindAsync(id);
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(string id)
        {
            var item = await _db.Customers.FindAsync(id);

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
