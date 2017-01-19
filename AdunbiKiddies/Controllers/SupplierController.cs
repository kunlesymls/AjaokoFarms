using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class SupplierController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Supplier
        public async Task<ActionResult> Index()
        {
            return View(await db.Suppliers.ToListAsync());
        }

        // GET: Supplier/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Supplier/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Products
            //ViewBag.CatagorieId = new SelectList(db.Categories, "ID", "Name", product.CategoriesId);
            ViewBag.ProductSupplied = new SelectList(await db.Products.ToListAsync(), "Name", "Name");
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
