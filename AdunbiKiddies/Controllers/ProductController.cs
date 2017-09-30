using AdunbiKiddies.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class ProductController : BaseController
    {
        private AjaoOkoDb db = new AjaoOkoDb();

        // GET: Product
        public ActionResult Index(string category, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = _db.Products.Include(i => i.Merchant).Where(x => x.Merchant.IsVerified.Equals(true));
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                         || s.Category.Name.ToUpper().Contains(searchString.ToUpper())
                                         || s.AlternativeName.ToUpper().Contains(searchString.ToUpper()));
            }
            else if (!String.IsNullOrEmpty(category))
            {
                items = items.Where(s => s.Category.Name.ToUpper().Contains(category.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:  // Name ascending 
                    items = items.OrderBy(s => s.Name);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));

            //var items = db.Items.Include(i => i.Catagorie);
            //return View(await items.ToListAsync());
        }
        // GET: Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public PartialViewResult CreateReview()
        {
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "MerchantId");
            return PartialView();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.MerchantId = new SelectList(db.Merchants, "MerchantId", "CompanyName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,MerchantId,CategoryId,Name,AlternativeName,Price,Quantity,Unit,OtherUnitName,Description,ProductDiscount,DiscountPrice,IsApproved,DateAdded,StockQuantity,InternalImage,ItemPictureUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,MerchantId,CategoryId,Name,AlternativeName,Price,Quantity,Unit,OtherUnitName,Description,ProductDiscount,DiscountPrice,IsApproved,DateAdded,StockQuantity,InternalImage,ItemPictureUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.MerchantId = new SelectList(db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
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
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(int ProductId)
        {
            Product item = await _db.Products.FindAsync(ProductId);

            byte[] photoBack = item.InternalImage;

            return File(photoBack, "image/png");
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
