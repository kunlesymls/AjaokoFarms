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
    public class MerchantPaymentsController : BaseController
    {
        // GET: MerchantPayments
        public async Task<ActionResult> Index()
        {
            var merchantPayments = _db.MerchantPayments.Include(m => m.Merchant);
            return View(await merchantPayments.ToListAsync());
        }

        // GET: MerchantPayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchantPayment merchantPayment = await _db.MerchantPayments.FindAsync(id);
            if (merchantPayment == null)
            {
                return HttpNotFound();
            }
            return View(merchantPayment);
        }

        // GET: MerchantPayments/Create
        // GET: ProfessionalPayments/Create
        public async Task<ActionResult> Create()
        {
            var userId = User.Identity.GetUserId();
            var merchants = await _db.Merchants.AsNoTracking()
                            .Where(x => x.MerchantId.Equals(userId))
                            .FirstOrDefaultAsync();
            var model = new MerchantPaymentVm
            {
                Amount = 5000,
                MerchantId = userId,
                MerchantName = merchants.FullName
            };

            return View(model);
        }

        // POST: MerchantPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MerchantPaymentVm model)
        {
            if (ModelState.IsValid)
            {
                var merchantId = User.Identity.GetUserId();

                var merchant = await _db.Merchants.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MerchantId.Equals(merchantId));

                var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
                var api = new PayStackApi(testOrLiveSecret);
                // Initializing a transaction
                //var response = api.Transactions.Initialize("user@somewhere.net", 5000000);
                var transactionInitializaRequest = new TransactionInitializeRequest
                {
                    //Reference = "SwifKampus",
                    AmountInKobo = _query.ConvertToKobo((int)model.Amount),
                    //CallbackUrl = "https://unibenportal.azurewebsites.net/SchoolFeePayments/ConfrimPayment",
                    CallbackUrl = "http://localhost:59969/MerchantPayments/ConfrimPayment",
                    Email = merchant.Email,
                    Bearer = "Application fee",

                    CustomFields = new List<CustomField>
                    {
                        new CustomField("merchantid","merchantid", merchant.MerchantId),
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

        // GET: MerchantPayments/ConfrimPayment
        public async Task<ActionResult> ConfrimPayment(string reference)
        {
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            //Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
            if (verifyResponse.Status)
            {
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
                var merchantId = User.Identity.GetUserId();
                var merchantPayment = new MerchantPayment
                {

                    MerchantId = convertedValues.Where(x => x.key.Equals("merchantid")).Select(s => s.value).FirstOrDefault(),
                    PaymentDateTime = DateTime.Now,
                    ExpectedAmount = Convert.ToDecimal(convertedValues.Where(x => x.key.Equals("amount")).Select(s => s.value).FirstOrDefault()),
                    IsPayed = true,
                    //StudentId = "HAS-201",
                    AmountPayed = _query.ConvertToNaira(verifyResponse.Data.Amount),

                };
                _db.MerchantPayments.Add(merchantPayment);
                var editMerchant = await _db.Merchants.Where(x => x.MerchantId.Equals(merchantId)).FirstOrDefaultAsync();
                editMerchant.ExpiryDate = DateTime.Now.AddDays(365);
                editMerchant.Haspayed = true;
                _db.Entry(editMerchant).State = EntityState.Modified;


                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: MerchantPayments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchantPayment merchantPayment = await _db.MerchantPayments.FindAsync(id);
            if (merchantPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", merchantPayment.MerchantId);
            return View(merchantPayment);
        }

        // POST: MerchantPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MerchantPaymentId,MerchantId,AmountPayed,ExpectedAmount,PaymentDateTime,IsPayed,Status")] MerchantPayment merchantPayment)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(merchantPayment).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MerchantId = new SelectList(_db.Merchants, "MerchantId", "CompanyName", merchantPayment.MerchantId);
            return View(merchantPayment);
        }

        // GET: MerchantPayments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchantPayment merchantPayment = await _db.MerchantPayments.FindAsync(id);
            if (merchantPayment == null)
            {
                return HttpNotFound();
            }
            return View(merchantPayment);
        }

        // POST: MerchantPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MerchantPayment merchantPayment = await _db.MerchantPayments.FindAsync(id);
            _db.MerchantPayments.Remove(merchantPayment);
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
