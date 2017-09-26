using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class ProductController : BaseController
    {

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var products = _db.Products.Include(p => p.Category).Include(p => p.Merchant);
            return View(await products.ToListAsync());
        }

        public async Task<ActionResult> ProductsView()
        {
            var products = _db.Products.Include(p => p.Category).Include(p => p.Merchant);
            return View(await products.ToListAsync());
        }



        public async Task<ActionResult> Approve()
        {
            ViewBag.Message = "";

            return View(await _db.Products.Where(m => m.IsApproved == false).ToListAsync());

        }


        public async Task<ActionResult> Approved(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                ViewBag.Message = "Please select a Merchant";
                return RedirectToAction("Approve");
            }
            //_db.Merchants.Attach(merchant);

            product.IsApproved = true;
            _db.Entry(product).State = EntityState.Modified;

            _db.SaveChanges();

            ViewBag.Message = "Merchant Approval successful";
            return RedirectToAction("Approve");
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "FullName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "FullName", product.MerchantId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "FullName", product.MerchantId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "FullName", product.MerchantId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            _db.Products.Remove(product);
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
