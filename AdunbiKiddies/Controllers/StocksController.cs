using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class StocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stocks
        public async Task<ActionResult> Index()
        {
            return View(await db.Stocks.ToListAsync());
        }

        // GET: Stocks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = await db.Stocks.FindAsync(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            ViewBag.Name = new SelectList(db.Products, "ID", "Name");
            ViewBag.StaffName = User.Identity.GetUserName().ToString();
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Quantity,Date,StaffName,Status")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                //var position = db.Grades.Include(g => g.Course).Include(g => g.Student)
                //             .Where(s => s.CourseCode.Equals("STA") && s.Student.MyClassName.Equals("SS2")
                //              && s.TermName.Equals("Second") && s.SessionName.Equals("2016/2017"))
                //           .OrderBy(d => d.Total);

                //Product position = db.Products.

                Product product = await db.Products.FindAsync(int.Parse(stock.Name));

                if (product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    int incomingValue = stock.Quantity;

                    if (product.StockQuantity == null)
                    {
                        product.StockQuantity = 0;
                    }

                    if (stock.Status.Equals(PopUp.Status.Add))
                    {
                        product.StockQuantity += incomingValue;
                    }
                    else if (stock.Status.Equals(PopUp.Status.Remove))
                    {
                        product.StockQuantity -= incomingValue;
                    }


                }
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(stock);
        }


        //// GET: Stocks/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Stock stock = await db.Stocks.FindAsync(id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(stock);
        //}

        //// POST: Stocks/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Quantity,Date,StaffName")] Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(stock).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(stock);
        //}

        //// GET: Stocks/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Stock stock = await db.Stocks.FindAsync(id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(stock);
        //}

        //// POST: Stocks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Stock stock = await db.Stocks.FindAsync(id);
        //    db.Stocks.Remove(stock);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
