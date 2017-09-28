using AdunbiKiddies.Models;
using AdunbiKiddies.SMS_Service;
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
    //[Authorize]
    public class CheckoutController : BaseController
    {
        private AjaoOkoDb storeDB;
        private readonly SmsServiceTemp _smsService;

        public CheckoutController()
        {
            _smsService = new SmsServiceTemp();
            storeDB = new AjaoOkoDb();
        }

        //AppConfigurations appConfig = new AppConfigurations();

        //public List<String> CreditCardTypes { get { return appConfig.CreditCardType; } }

        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Address()
        {
            return PartialView();

        }
        public PartialViewResult Shiping()
        {
            return PartialView();
        }
        public PartialViewResult ReviewCart()
        {
            return PartialView();
        }

        //GET: /Checkout/AddressAndPayment
        public async Task<ActionResult> AddressAndPayment()
        {
            var cId = User.Identity.GetUserId();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cDetails = await storeDB.ShippingDetails.Where(x => x.CustomerId.Equals(cId)).FirstOrDefaultAsync();
            if (cDetails != null)
            {
                var payment = new OrderPaymentVm
                {
                    CustomerId = cDetails.CustomerId,
                    CustomerName = cDetails.Customer.FullName,
                    Email = cDetails.Customer.Email,
                    TotalAmount = cart.GetTotal(),
                    ShippingDetail = cDetails,
                    //SaleId = Convert.ToInt32(cart.ShoppingCartId)
                };
                return View(payment);
            }
            //return RedirectToAction("Create", "ShippingDetails");
            return View();

        }

        public async Task<ActionResult> MakePayment(OrderPaymentVm model)
        {
            if (ModelState.IsValid)
            {
                //var totalSale = await storeDB.Sales.AsNoTracking().CountAsync();
                //var saleId = totalSale + 1;


                var sale = new Sale
                {
                    CustomerId = model.CustomerId,
                    Total = model.TotalAmount,
                    IsPayed = false,
                    SaleDate = DateTime.Now
                };
                storeDB.Sales.Add(sale);
                await storeDB.SaveChangesAsync();

                //Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                sale = cart.CreateOrder(sale);
                var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
                var api = new PayStackApi(testOrLiveSecret);
                // Initializing a transaction
                //var response = api.Transactions.Initialize("user@somewhere.net", 5000000);
                var transactionInitializaRequest = new TransactionInitializeRequest
                {
                    //Reference = "SwifKampus",
                    AmountInKobo = _query.ConvertToKobo((int)model.TotalAmount),
                    CallbackUrl = "http://localhost:59969/Checkout/ConfrimPayment",
                    Email = model.Email,
                    Bearer = "Ajaoko Order Payment",

                    CustomFields = new List<CustomField>
                    {
                        new CustomField("customerid","customerid", model.CustomerId),
                        new CustomField("amount", "amount", model.TotalAmount.ToString(CultureInfo.InvariantCulture)),
                        new CustomField("saleid", "saleid", sale.SaleId.ToString())
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
            return RedirectToAction("AddressAndPayment");
        }

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
                //var studentid = _db.Users.Find(id);
                var saleId = Convert.ToInt32(convertedValues.Where(x => x.key.Equals("saleid")).Select(s => s.value).FirstOrDefault());
                var payment = new OrderPayment()
                {
                    CustomerId = convertedValues.Where(x => x.key.Equals("customerid")).Select(s => s.value).FirstOrDefault(),
                    PaymentDateTime = DateTime.Now,
                    Amount = Convert.ToDecimal(convertedValues.Where(x => x.key.Equals("amount")).Select(s => s.value).FirstOrDefault()),
                    IsPayed = true,
                    SaleId = saleId,
                    AmountPaid = _query.ConvertToNaira(verifyResponse.Data.Amount),

                };
                _db.OrderPayments.Add(payment);
                _db.SaveChanges();

                var sale = await storeDB.Sales.AsNoTracking().Where(x => x.SaleId.Equals(saleId)).FirstOrDefaultAsync();
                if (sale != null)
                {
                    sale.IsPayed = true;
                    _db.Entry(sale).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                return RedirectToAction("Details", "Sales", new { id = saleId });
            }

            return RedirectToAction("AddressAndPayment");
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            var sale = new Sale();
            TryUpdateModel(sale);
            //order.CreditCard = result;

            try
            {
                sale.CustomerId = User.Identity.GetUserId();
                sale.SaleDate = DateTime.Now;
                var currentUserId = User.Identity.GetUserId();

                //Save Order
                storeDB.Sales.Add(sale);
                await storeDB.SaveChangesAsync();
                //Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                sale = cart.CreateOrder(sale);

                //CheckoutController.SendOrderMessage(sale.FirstName, "New Order: " + sale.SaleId, sale.ToString(sale));
                string body = "Thanks for patronizing us, We will get in touch with you soon";
                SendMessage(body, sale.Customer.PhoneNumber);
                return RedirectToAction("Complete",
                        new { id = sale.SaleId });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View("AddressAndPayment");
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Sales.Any(
                o => o.SaleId == id &&
                o.CustomerId == User.Identity.GetUserId());

            if (isValid)
            {
                return RedirectToAction("PrintDetails",
                      new { id = id });
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> PrintDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await storeDB.Sales.FindAsync(id);
            var saleDetails = storeDB.SaleDetails.Where(x => x.SaleId == id);

            sales.SaleDetails = await saleDetails.ToListAsync();
            if (sales == null)
            {
                return HttpNotFound();
            }

            return View(sales);
        }

        private void SendMessage(string body, string phoneNumber)
        {
            Sms sms = new Sms()
            {
                Sender = "AJAOKO",
                Message = body,
                Recipient = phoneNumber
            };
            string response = _smsService.Send(sms);
        }

        //private static RestResponse SendOrderMessage(String toName, String subject, String body, String destination)
        //{
        //    RestClient client = new RestClient();
        //    //fix this we have this up top too
        //    AppConfigurations appConfig = new AppConfigurations();
        //    client.BaseUrl = "https://api.mailgun.net/v2";
        //    client.Authenticator =
        //           new HttpBasicAuthenticator("api",
        //                                      appConfig.EmailApiKey);
        //    RestRequest request = new RestRequest();
        //    request.AddParameter("domain",
        //                        appConfig.DomainForApiKey, ParameterType.UrlSegment);
        //    request.Resource = "{domain}/messages";
        //    request.AddParameter("from", appConfig.FromName + " <" + appConfig.FromEmail + ">");
        //    request.AddParameter("to", toName + " <" + destination + ">");
        //    request.AddParameter("subject", subject);
        //    request.AddParameter("html", body);
        //    request.Method = Method.POST;
        //    IRestResponse executor = client.Execute(request);
        //    return executor as RestResponse;
        //}
    }
}