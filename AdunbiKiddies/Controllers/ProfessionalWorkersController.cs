using OpenOrderFramework.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdunbiKiddies.Models;

namespace AdunbiKiddies.Controllers
{
    public class ProfessionalWorkersController : BaseController
    {
        //  private AjaoOko_db _db = new AjaoOko_db();

        // GET: ProfessionalWorkers
        public ActionResult Index()
        {
            return View(_db.ProfessionalWorkers.ToList());
        }
        public ActionResult ProWorkerView()
        {
            return View(_db.ProfessionalWorkers.ToList());
        }

        public async Task<ActionResult> Approve()
        {
            ViewBag.Message = "";
            return View(await _db.ProfessionalWorkers.Where(m => m.IsVerified == false).ToListAsync());

        }

        [HttpPost]
        public async Task<ActionResult> Approve(int id)
        {
            ProfessionalWorker pWorker = await _db.ProfessionalWorkers.FindAsync(id);
            if (pWorker == null)
            {
                ViewBag.Message = "Please select a Merchant";
                return View();
            }
            //_db.Merchants.Attach(merchant);

            pWorker.IsVerified = true;
            _db.Entry(pWorker).Property(x => x.IsVerified).IsModified = true;

            _db.SaveChanges();

            ViewBag.Message = "Merchant Approval successful";
            return View();
        }

        // GET: ProfessionalWorkers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorker professionalWorker = _db.ProfessionalWorkers.Find(id);
            if (professionalWorker == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorker);
        }

        // GET: ProfessionalWorkers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfessionalWorkers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfessionalWorkerId,HighestQualification,FirstName,MiddleName,LastName,Gender,Email,PhoneNumber,TownOfBirth,StateOfOrigin,Nationality,Passport")] ProfessionalWorker professionalWorker)
        {
            if (ModelState.IsValid)
            {
                _db.ProfessionalWorkers.Add(professionalWorker);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(professionalWorker);
        }

        // GET: ProfessionalWorkers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorker professionalWorker = _db.ProfessionalWorkers.Find(id);
            if (professionalWorker == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorker);
        }

        // POST: ProfessionalWorkers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfessionalWorker professionalWorker)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(professionalWorker).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professionalWorker);
        }

        // GET: ProfessionalWorkers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorker professionalWorker = _db.ProfessionalWorkers.Find(id);
            if (professionalWorker == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorker);
        }

        // POST: ProfessionalWorkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProfessionalWorker professionalWorker = _db.ProfessionalWorkers.Find(id);
            _db.ProfessionalWorkers.Remove(professionalWorker);
            _db.SaveChanges();
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
