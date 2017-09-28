using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class ProductReviewsController : BaseController
    {

        // GET: ProductReviews
        public async Task<ActionResult> Index()
        {
            var productReviews = _db.ProductReviews.Include(p => p.Product);
            return View(await productReviews.ToListAsync());
        }

        // GET: ProductReviews/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductReview productReview = await _db.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return HttpNotFound();
            }
            return View(productReview);
        }

        // GET: ProductReviews/Create
        //public PartialViewResult Create()
        //{
        //    ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "MerchantId");
        //    return PartialView();
        //}

        // POST: ProductReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        public async Task<ActionResult> Create(ProductReviewVm model)
        {
            if (ModelState.IsValid)
            {
                var productReview = new ProductReview
                {
                    ProductId = model.ProductId,
                    Rating = model.rating,
                    Review = model.review,
                    UserName = model.username,
                    Email = model.email,
                    Subject = model.subject
                };
                _db.ProductReviews.Add(productReview);
                await _db.SaveChangesAsync();
                return new JsonResult { Data = new { status = true, message = "Product Review Saved Successfully" } };
            }

            return new JsonResult { Data = new { status = false, message = "Product review not Saved" } };

        }

        // GET: ProductReviews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductReview productReview = await _db.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "MerchantId", productReview.ProductId);
            return View(productReview);
        }

        // POST: ProductReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductReviewId,ProductId,Rating,UserName,Email,Subject,Review")] ProductReview productReview)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(productReview).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "MerchantId", productReview.ProductId);
            return View(productReview);
        }

        // GET: ProductReviews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductReview productReview = await _db.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return HttpNotFound();
            }
            return View(productReview);
        }

        // POST: ProductReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductReview productReview = await _db.ProductReviews.FindAsync(id);
            _db.ProductReviews.Remove(productReview);
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
