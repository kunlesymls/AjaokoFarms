using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdunbiKiddies.Models.Repository;
using AdunbiKiddies.ViewModels;
using AdunbiKiddies.Models;

namespace AdunbiKiddies.Controllers
{
    public class ProductSearchsController : BaseController
    {

        ProductRepo productRepo = new ProductRepo();
       // GET: ProductSearchs
       [HttpGet]
        public ActionResult SearchProduct()
        {
           TempData["SearchProduct"]= "";
            return View();
        }

        //GET: ProductSearchs/Details/5

        [HttpPost]
        public ActionResult SearchProduct(string txtSearchTerm)
        {
            
            try {
                string searchTerm = txtSearchTerm.ToString();
                int itemCount = productRepo.SearchProduct(searchTerm).Count();
                //productRepo.SearchProduct(searchTerm)
                if (itemCount > 0)
                {
                    TempData["SearchProduct"] = itemCount.ToString() + "results was matched your search";
                    var result = (productRepo.SearchProduct(searchTerm));
                   // ViewData.Model = result;
                    return View(result);
                }
                else
                {
                    TempData["SearchProduct"] = itemCount.ToString() + "results was matched your search";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception ex)
            {
                return View(ex.Message.ToString());
            }

            //return null;
        }

        public ActionResult SearchCat(int catID)
        {
            var result = productRepo.GetProductByCat(catID);
            int itemCount = result.Count();
            if (itemCount > 0)
            {
                ViewBag.Message = itemCount + "Matching Records Found";
                return View("Index", result);
            }
            return View(result);
        }

        public ActionResult SearchPrice(decimal price)
        {

            var result = productRepo.GetByPrice(price);
            int itemCount = result.Count();
            if (itemCount > 0)
            {
                ViewBag.Message = itemCount + "Matching Records Found";
                return View("Index", result);
            }
            return View(result);
        }



        // GET: ProductSearchs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        //    ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName");
        //    return View();
        //}

        // POST: ProductSearchs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProductId,MerchantId,CategoryId,Name,AlternativeName,Price,Description,ProductDiscount,DiscountPrice,IsApproved,StockQuantity,InternalImage,ItemPictureUrl")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Products.Add(product);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
        //    ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
        //    return View(product);
        //}

        //// GET: ProductSearchs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = _db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
        //    ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
        //    return View(product);
        //}

        //// POST: ProductSearchs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProductId,MerchantId,CategoryId,Name,AlternativeName,Price,Description,ProductDiscount,DiscountPrice,IsApproved,StockQuantity,InternalImage,ItemPictureUrl")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Entry(product).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
        //    ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", product.MerchantId);
        //    return View(product);
        //}

        //// GET: ProductSearchs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = _db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        //// POST: ProductSearchs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Product product = _db.Products.Find(id);
        //    _db.Products.Remove(product);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
