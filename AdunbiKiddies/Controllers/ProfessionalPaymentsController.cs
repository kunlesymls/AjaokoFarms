using AdunbiKiddies.Models;
using AdunbiKiddies.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class ProfessionalPaymentsController : BaseController
    {
        // GET: ProfessionalPayments
        public async Task<ActionResult> Index()
        {
            var professionalPayments = _db.ProfessionalPayments.Include(p => p.ProfessionalWorker);
            return View(await professionalPayments.ToListAsync());
        }

        // GET: ProfessionalPayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalPayment professionalPayment = await _db.ProfessionalPayments.FindAsync(id);
            if (professionalPayment == null)
            {
                return HttpNotFound();
            }
            return View(professionalPayment);
        }

        // GET: ProfessionalPayments/Create
        public async Task<ActionResult> Create()
        {
            var userId = User.Identity.GetUserId();
            var worker = await _db.ProfessionalWorkers.AsNoTracking()
                            .Where(x => x.ProfessionalWorkerId.Equals(userId))
                            .ToListAsync();
            ViewBag.ProfessionalWorkerId = new SelectList(worker, "ProfessionalWorkerId", "FullName");
            return View();
        }

        // POST: ProfessionalPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProfessionalPayment model)
        {
            if (ModelState.IsValid)
            {
                var workerId = User.Identity.GetUserId();

                var worker = await _db.ProfessionalWorkers.AsNoTracking()
                                .FirstOrDefaultAsync(x => x.ProfessionalWorkerId.Equals(workerId));

                var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
                var api = new PayStackApi(testOrLiveSecret);
                // Initializing a transaction
                //var response = api.Transactions.Initialize("user@somewhere.net", 5000000);
                var transactionInitializaRequest = new TransactionInitializeRequest
                {
                    //Reference = "SwifKampus",
                    AmountInKobo = _query.ConvertToKobo((int)model.Amount),
                    //CallbackUrl = "https://unibenportal.azurewebsites.net/SchoolFeePayments/ConfrimPayment",
                    CallbackUrl = "http://localhost:59969/ProfessionalPayments/ConfrimPayment",
                    Email = worker.Email,
                    Bearer = "Application fee",

                    CustomFields = new List<CustomField>
                    {
                        new CustomField("professionalworkerid","professionalworkerid", worker.ProfessionalWorkerId),
                        new CustomField("amount", "amount", model.Amount.ToString(CultureInfo.InvariantCulture))
                        //new CustomField("sessionid", "sessionid", schoolFeePayment.SessionId.ToString()),
                        //new CustomField("feepaymentid","feepaymentid", schoolFeePayment.FeeCategoryId.ToString())
                        //new CustomField("paymentmode","paymentmode", schoolFeePayment.p)
                    }

                };
                var response = api.Transactions.Initialize(transactionInitializaRequest);

                if (response.Status)
                {
                    //redirect to authorization url
                    return RedirectPermanent(response.Data.AuthorizationUrl);
                    // return Content("Successful");
                }
                return Content("An error occurred");

            }
            //return View(schoolFeePayment);
            return RedirectToAction("Create");
        }

        // GET: ProfessionalPayments/ConfrimPayment
        public async Task<ActionResult> ConfrimPayment(string reference)
        {
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            //Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
            if (verifyResponse.Status)
            {
                /* 
                   You can save the details from the json object returned above so that the authorization code 
                   can be used for charging subsequent transactions

                   // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
                   // Save 'authCode' for future charges!
                   
               */
                //var customfieldArray = verifyResponse.Data.Metadata.CustomFields.A

                var convertedValues = new List<SelectableEnumItem>();
                var valuepair = verifyResponse.Data.Metadata.Where(x => x.Key.Contains("custom")).Select(s => s.Value);

                foreach (var item in valuepair)
                {
                    convertedValues = ((JArray)item).Select(x => new SelectableEnumItem
                    {
                        key = (string)x["display_name"],
                        value = (string)x["value"]
                    }).ToList();
                }
                //var studentid = _db.Users.Find(id);
                var professionalPayment = new ProfessionalPayment()
                {
                    //FeeCategoryId = Convert.ToInt32(verifyResponse.Data.Metadata.CustomFields[3].Value),
                    ProfessionalWorkerId = convertedValues.Where(x => x.key.Equals("professionalworkerid")).Select(s => s.value).FirstOrDefault(),
                    PaymentDateTime = DateTime.Now,
                    Amount = Convert.ToDecimal(convertedValues.Where(x => x.key.Equals("amount")).Select(s => s.value).FirstOrDefault()),
                    IsPayed = true,
                    //StudentId = "HAS-201",
                    AmountPaid = _query.ConvertToNaira(verifyResponse.Data.Amount),

                };
                _db.ProfessionalPayments.Add(professionalPayment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: ProfessionalPayments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalPayment professionalPayment = await _db.ProfessionalPayments.FindAsync(id);
            if (professionalPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfessionalWorkerId = new SelectList(_db.ProfessionalWorkers, "ProfessionalWorkerId", "HighestQualification", professionalPayment.ProfessionalWorkerId);
            return View(professionalPayment);
        }

        // POST: ProfessionalPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProfessionalPaymentId,ProfessionalWorkerId,Amount,AmountPaid")] ProfessionalPayment professionalPayment)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(professionalPayment).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProfessionalWorkerId = new SelectList(_db.ProfessionalWorkers, "ProfessionalWorkerId", "HighestQualification", professionalPayment.ProfessionalWorkerId);
            return View(professionalPayment);
        }

        // GET: ProfessionalPayments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalPayment professionalPayment = await _db.ProfessionalPayments.FindAsync(id);
            if (professionalPayment == null)
            {
                return HttpNotFound();
            }
            return View(professionalPayment);
        }

        // POST: ProfessionalPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProfessionalPayment professionalPayment = await _db.ProfessionalPayments.FindAsync(id);
            _db.ProfessionalPayments.Remove(professionalPayment);
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
