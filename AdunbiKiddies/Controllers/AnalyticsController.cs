using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdunbiKiddies.Models;
using AdunbiKiddies.ViewModels;

namespace AdunbiKiddies.Controllers
{
    public class AnalyticsController : Controller
    {
        private AjaoOkoDb db = new AjaoOkoDb();
        AnalyticsViewModel vm = new AnalyticsViewModel();

        // GET: Analytics
        public async Task<ActionResult> Index()
        {
            var data = (from sales in db.Sales
                       group sales by sales.SaleDate into dateGroup
                       select new SaleDateGroup()
                       {
                            SaleDate = dateGroup.Key,
                            SaleCount = dateGroup.Count()
                       }).Take(10);

            var allData = (from sales in db.Sales
                        group sales by sales.SaleDate into dateGroup
                        select new SaleDateGroup()
                        {
                            SaleDate = dateGroup.Key,
                            SaleCount = dateGroup.Count()
                        });

            
            vm.SaleDate = await data.ToListAsync();
            vm.SaleDataForToday = await allData.ToListAsync();

            // Commenting out LINQ to show how to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
            //string query = "SELECT OrderDate, COUNT(*) AS StudentCount "
            //    + "FROM Person "
            //    + "WHERE Discriminator = 'Student' "
            //    + "GROUP BY EnrollmentDate";
            //IEnumerable<Order> data = db.Database.SqlQuery<Order>(query);


            return View(vm);
        }

        // GET: Analytics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Analytics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Analytics/Create
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

            return View(sale);
        }

        // GET: Analytics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Analytics/Edit/5
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
            return View(sale);
        }

        // GET: Analytics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale order = await db.Sales.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Analytics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sale = await db.Sales.FindAsync(id);
            db.Sales.Remove(sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public JsonResult GetDataAsJson()
        {
            var data = GetData();
            return Json(data, JsonRequestBehavior.AllowGet);
            //return data;
        }

        private List<SaleDateGroup> GetData()
        {
            var allData = (from sales in db.Sales
                           group sales by sales.SaleDate into dateGroup
                           select new SaleDateGroup()
                           {
                               SaleDate = dateGroup.Key,
                               SaleCount = dateGroup.Count()
                           });


            var OrderData = allData.ToList();

            return OrderData;
        }

        public dynamic StopsByMonth()
        {
            var resultSet = GetData();
            return resultSet;
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
