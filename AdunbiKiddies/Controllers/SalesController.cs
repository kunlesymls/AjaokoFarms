using AdunbiKiddies.Models;
using AdunbiKiddies.SMS_Service;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    [Authorize]
    public class SalesController : BaseController
    {


        // GET: Sales
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
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
            string checkName = User.Identity.GetUserId();

            IEnumerable<Sale> sales = new List<Sale>();

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                sales = await _db.Sales.Include(i => i.Customer).AsNoTracking().ToListAsync();
            }
            else
            {
                sales = _db.Sales.Include(i => i.Customer).AsNoTracking().Where(s => s.CustomerId.Equals(checkName));
            }

            //var sales = from o in _db.Sales
            //            select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                sales = sales.Where(s => s.Customer.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.Customer.UserName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sales = sales.OrderByDescending(s => s.Customer.UserName);
                    break;

                case "Price":
                    sales = sales.OrderBy(s => s.Total);
                    break;

                case "price_desc":
                    sales = sales.OrderByDescending(s => s.Total);
                    break;

                default:  // Name ascending
                    sales = sales.OrderBy(s => s.Customer.UserName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));

            //return View(await _db.Orders.ToListAsync());
        }

        public async Task<ActionResult> DailySales(string sortOrder, string currentFilter, DailySales dailysales, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            string searchString = String.Empty;
            if (dailysales.Date != null)
            {
                var searchdate = (DateTime)dailysales.Date;
                searchString = searchdate.ToShortDateString();
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            string checkName = User.Identity.GetUserId();
            var todayDate = DateTime.Now.ToString("dd/MM/yyyy");

            IEnumerable<Sale> sales = new List<Sale>();

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                sales = await _db.Sales.AsNoTracking().ToListAsync();
            }
            else
            {
                sales = _db.Sales.AsNoTracking().Where(s => s.CustomerId.Equals(checkName));
            }

            //var sales = from o in _db.Sales
            //            select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                // var salesresult = sales.Where(s => s.SaleDate.Date.ToString());
                sales = sales.Where(s => s.SaleDate.ToShortDateString().Equals(searchString));
            }
            else
            {
                sales = sales.Where(x => x.SaleDate.ToString("dd/MM/yyyy").Equals(todayDate));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sales = sales.OrderByDescending(s => s.Customer.UserName);
                    break;

                case "Price":
                    sales = sales.OrderBy(s => s.Total);
                    break;

                case "price_desc":
                    sales = sales.OrderByDescending(s => s.Total);
                    break;

                default:  // Name ascending
                    sales = sales.OrderBy(s => s.Customer.UserName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));

            //return View(await _db.Orders.ToListAsync());
        }

        public ActionResult DailyDate()
        {
            return PartialView();
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await _db.Sales.FindAsync(id);
            var saleDetails = _db.SaleDetails.Where(x => x.SaleId == id);

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
                _db.Sales.Add(sale);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            return View(sale);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await _db.Sales.FindAsync(id);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(sale).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SalesRepName = User.Identity.GetUserName();
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.SaleDate = datetime.ToShortDateString();
            return View(sale);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sales = await _db.Sales.FindAsync(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sales = await _db.Sales.FindAsync(id);
            _db.Sales.Remove(sales);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task DownloadDailyReport()
        {
            //var facilityList = _db.Communications.AsNoTracking().ToList();
            string productBuilder = String.Empty;
            string quantityBuilder = String.Empty;
            string repName = User.Identity.GetUserId();

            var dailySales = await _db.Sales.Include(i => i.SaleDetails).Include(i => i.Customer).AsNoTracking()
                .Where(x => x.SaleDate.Year == DateTime.Now.Year
                            && x.SaleDate.Month == DateTime.Now.Month
                            && x.SaleDate.Day == DateTime.Now.Day)
                .OrderBy(o => o.SaleDate).ToListAsync();
            if (Request.IsAuthenticated && User.IsInRole("SalesRep"))
            {
                dailySales.Where(x => x.CustomerId.Equals(repName));
            }
            decimal total = 0;
            char c1 = 'A';
            var todayDate = DateTime.Now.ToShortDateString();
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Report for {todayDate}");

            worksheet.Cells[$"{c1++}1"].Value = "Sales Rep Name";
            worksheet.Cells[$"{c1++}1"].Value = "Customer's Name";
            worksheet.Cells[$"{c1++}1"].Value = "Phone Number";
            worksheet.Cells[$"{c1++}1"].Value = "Items bought";
            worksheet.Cells[$"{c1++}1"].Value = "Quanties";
            worksheet.Cells[$"{c1++}1"].Value = "Total";

            int rowStart = 2;
            char c2 = 'A';

            foreach (Sale sales in dailySales)
            {
                worksheet.Cells[$"A{rowStart}"].Value = sales.Customer.Email;
                worksheet.Cells[$"B{rowStart}"].Value = $"{sales.Customer.FullName}";
                worksheet.Cells[$"C{rowStart}"].Value = sales.Customer.PhoneNumber;
                foreach (var product in sales.SaleDetails.OrderBy(o => o.ProductId))
                {
                    productBuilder += $"{product.Product.Name}, ";
                    quantityBuilder += ($"{product.Quantity}, ");
                    total += product.Quantity * product.UnitPrice;
                }
                worksheet.Cells[$"D{rowStart}"].Value = productBuilder;
                worksheet.Cells[$"E{rowStart}"].Value = quantityBuilder;
                worksheet.Cells[$"F{rowStart}"].Value = total.ToString(CultureInfo.CurrentCulture);
                rowStart++;
                total = 0;
                productBuilder = String.Empty;
                quantityBuilder = string.Empty;
            }
            // var info = results.FirstOrDefault();
            worksheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"DailySales.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }

        public async Task<ActionResult> NotifyByEmail()
        {
            string productBuilder = String.Empty;
            string quantityBuilder = String.Empty;
            decimal total = 0;

            var emailVm = new EmailVm();
            var dailySales = await _db.Sales.Include(i => i.SaleDetails).Include(i => i.Customer).AsNoTracking()
                .Where(x => x.SaleDate.Year == DateTime.Now.Year
                            && x.SaleDate.Month == DateTime.Now.Month
                            && x.SaleDate.Day == DateTime.Now.Day)
                .OrderBy(o => o.SaleDate).ToListAsync();

            var rows = new StringBuilder();
            string SalesBody = "<strong> Hello Ma, </strong><br />" +
                              $"<strong>Daily Sales Report for {DateTime.Now.ToShortDateString()} </strong>" +
                              "<br > Please find can find breakup below " +
                              "<table border=\"1\">" +
                              "<tr>" +
                              "<th>Sales Rep Name</td>" +
                              "<th>Customer's Name</td>" +
                              "<th>Phone Number</td>" +
                              "<th>Items Bought</td>" +
                              "<th>Quantity Bought</td>" +
                              "<th>Amount</td>" +
                              "</tr>";
            rows.Append(SalesBody);
            foreach (Sale sales in dailySales)
            {
                foreach (var product in sales.SaleDetails.OrderBy(o => o.ProductId))
                {
                    productBuilder += $"{product.Product.Name}, ";
                    quantityBuilder += ($"{product.Quantity}, ");
                    total += product.Quantity * product.UnitPrice;
                }
                rows.Append("<tr>" +
                            $"<td>{sales.Customer.Email}</td>" +
                            $"<td>{sales.Customer.FullName}</td>" +
                            $"<td>{sales.Customer.PhoneNumber}</td>" +
                            $"<td>{productBuilder}</td>" +
                            $"<td>{quantityBuilder}</td>" +
                            $"<td>{total}</td>" +
                            "</tr>");

                productBuilder = String.Empty;
                quantityBuilder = String.Empty;
                total = 0;
            }
            rows.Append("</table>");

            var items = await _db.Products.AsNoTracking().Include(i => i.Category).ToListAsync();
            string itemLeft = "<br/><br ><br >" +
                              $"<strong>List of Product/Items that are less than three in Stock as at {DateTime.Now.ToShortDateString()}</strong>" +
                              "<br > you can find breakup below " +
                              "<table border=\"1\">" +
                              "<tr>" +
                              "<th>Product Name</td>" +
                              "<th>Section</td>" +
                              "<th>Product Category</td>" +
                              "<th>Quantity Remaining</td>" +
                              "</tr>";
            rows.Append(itemLeft);

            foreach (var product in items.Where(s => s.StockQuantity <= 3))
            {
                rows.Append("<tr>" +
                            $"<td>{product.Name}</td>" +
                            $"<td>{product.Category.StoreSection.SectionName}</td>" +
                            $"<td>{product.Category.Name}</td>" +
                            $"<td>{product.StockQuantity}</td>" +
                            "</tr>");
            }
            rows.Append("</table>");
            string finishItem = "<br/><br ><br >" +
                              $"<strong>List of Product/Items that is finished from Stock {DateTime.Now.ToShortDateString()}</strong>" +
                              "<br > you can find breakup below " +
                              "<table border=\"1\">" +
                              "<tr>" +
                              "<th>Product Name</td>" +
                              "<th>Section</td>" +
                              "<th>Product Category</td>" +
                              "<th>Quantity Remaining</td>" +
                              "</tr>";
            rows.Append(finishItem);

            foreach (var product in items.Where(s => s.StockQuantity <= 0))
            {
                rows.Append("<tr>" +
                            $"<td>{product.Name}</td>" +
                            $"<td>{product.Category.StoreSection.SectionName}</td>" +
                            $"<td>{product.Category.Name}</td>" +
                            $"<td>{product.StockQuantity}</td>" +
                            "</tr>");
            }
            rows.Append("</table>");

            emailVm.Body = rows.ToString();
            emailVm.Destination = "kunlesymls@gmail.com";
            emailVm.Subject = $"Sales report for {DateTime.Now.ToShortDateString()} for Amudu Bello Office";

            await SendEmailAsync(emailVm);
            return RedirectToAction("Index", "Home");
        }

        public async Task SendEmailAsync(EmailVm message)
        {
            // Plug in your email service here to send an email.
            string schoolName = "Designer's Choice";
            string emailsetting = ConfigurationManager.AppSettings["GmailUserName"];
            MailMessage email = new MailMessage(new MailAddress($"noreply{emailsetting}", $"(Daily Sales for {DateTime.Now.ToShortDateString()}, do not reply)"),
                new MailAddress(message.Destination));

            email.Subject = message.Subject;
            email.Body = message.Body;

            email.IsBodyHtml = true;

            using (var mailClient = new EmailSetUpServices())
            {
                //In order to use the original from email address, uncomment this line:
                email.From = new MailAddress(mailClient.UserName, $"(do not reply)@{schoolName}");

                await mailClient.SendMailAsync(email);
            }
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