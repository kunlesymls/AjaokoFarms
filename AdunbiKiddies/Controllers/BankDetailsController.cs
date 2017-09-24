using AdunbiKiddies.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class BankDetailsController : BaseController
    {

        // GET: BankDetails
        public async Task<ActionResult> Index()
        {
            return View(await _db.BankDetails.ToListAsync());
        }

        // GET: BankDetails/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDetail bankDetail = await _db.BankDetails.FindAsync(id);
            if (bankDetail == null)
            {
                return HttpNotFound();
            }
            return View(bankDetail);
        }

        // GET: BankDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BankDetailId,UserId,AccountName,AccountNumber,BankName,AccountType,IsPrimary")] BankDetail bankDetail)
        {
            if (ModelState.IsValid)
            {
                _db.BankDetails.Add(bankDetail);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bankDetail);
        }

        // GET: BankDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDetail bankDetail = await _db.BankDetails.FindAsync(id);
            if (bankDetail == null)
            {
                return HttpNotFound();
            }
            return View(bankDetail);
        }

        // POST: BankDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BankDetailId,UserId,AccountName,AccountNumber,BankName,AccountType,IsPrimary")] BankDetail bankDetail)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(bankDetail).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bankDetail);
        }

        // GET: BankDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDetail bankDetail = await _db.BankDetails.FindAsync(id);
            if (bankDetail == null)
            {
                return HttpNotFound();
            }
            return View(bankDetail);
        }

        // POST: BankDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BankDetail bankDetail = await _db.BankDetails.FindAsync(id);
            _db.BankDetails.Remove(bankDetail);
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
