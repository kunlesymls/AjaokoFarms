using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

            var sales = from o in db.Sales
                        select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                sales = sales.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sales = sales.OrderByDescending(s => s.FirstName);
                    break;
                case "Price":
                    sales = sales.OrderBy(s => s.Total);
                    break;
                case "price_desc":
                    sales = sales.OrderByDescending(s => s.Total);
                    break;
                default:  // Name ascending 
                    sales = sales.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));

            //return View(await db.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await db.Sales.FindAsync(id);
            var saleDetails = db.SaleDetails.Where(x => x.SaleId == id);

            sales.SaleDetails = await saleDetails.ToListAsync();
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            return View(sale);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await db.Sales.FindAsync(id);
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            return View(sale);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await db.Sales.FindAsync(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sales = await db.Sales.FindAsync(id);
            db.Sales.Remove(sales);
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
