using AdunbiKiddies.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdunbiKiddies.Controllers
{
    //[Authorize]
    public class ProductsController : Controller
    {
        private AjaoOkoDb db = new AjaoOkoDb();

        // GET: Items
        public ActionResult Index(string category, string sortOrder, string currentFilter, string searchString, string barString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null || barString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = db.Products.AsNoTracking().Where(x => x.StockQuantity > 3);

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Category.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            else if (!String.IsNullOrEmpty(category))
            {
                items = items.Where(s => s.Category.Name.ToUpper().Contains(category.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name);
                    break;

                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;

                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;

                default:  // Name ascending
                    items = items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
            ;

            //var items = db.Items.Include(i => i.Catagorie);
            //return View(await items.ToListAsync());
        }

        public ActionResult AdminIndex(string category, string sortOrder, string currentFilter, string searchString, string barString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null || barString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = db.Products.AsNoTracking().ToList();


            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                         || s.Category.Name.ToUpper().Contains(searchString.ToUpper()))
                                         .ToList();
            }
            else if (!String.IsNullOrEmpty(category))
            {
                items = items.Where(s => s.Category.Name.ToUpper().Contains(category.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name).ToList();
                    break;

                case "Price":
                    items = items.OrderBy(s => s.Price).ToList();
                    break;

                case "price_desc":
                    items = items.OrderByDescending(s => s.Price).ToList();
                    break;

                default:  // Name ascending
                    items = items.OrderBy(s => s.Name).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
            ;

            //var items = db.Items.Include(i => i.Catagorie);
            //return View(await items.ToListAsync());
        }

        public async Task<ActionResult> ItemLeft()
        {
            var items = from i in db.Products
                        select i;

            items = items.Where(s => s.StockQuantity.Value <= 3);

            return View(await items.ToListAsync());

            //var items = db.Items.Include(i => i.Catagorie);
            //return View(await items.ToListAsync());
        }

        public ActionResult UploadProducts()
        {
            //ViewBag.CourseName = new SelectList(db.Courses, "CourseName", "CourseName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadProducts(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return RedirectToAction("UploadProducts", "Products");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/ExcelUploadedFile/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    // Read data from excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        var productCount = db.Products.AsNoTracking().Count();
                        productCount += 1;
                        int id = int.Parse(((Excel.Range)range.Cells[row, 1]).Text);
                        int cat = int.Parse(((Excel.Range)range.Cells[row, 2]).Text);
                        string name = ((Excel.Range)range.Cells[row, 3]).Text;
                        decimal price = decimal.Parse(((Excel.Range)range.Cells[row, 4]).Text);
                        string generatedBarcode = $"DC Cat{cat} item{productCount}";

                        //Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                        //byte[] generatedImage = ImageTobyteArray(barcode.Draw(generatedBarcode, 50));

                        Product newProduct = new Product
                        {
                            CategoryId = cat,
                            Name = name.Trim(),
                            Price = price,

                        };
                        db.Products.Add(newProduct);
                        await db.SaveChangesAsync();
                    }
                    workbook.Close(0);
                    application.Quit();
                    ViewBag.Message = "Success";
                    return View();
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("Index");
                }
            }
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = db.Categories.Select(s => s.Name)
                                                            .OrderBy(s => s);
            return PartialView(categories);
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product item = await db.Products.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public async Task<ActionResult> PrintBarCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product item = await db.Products.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        //[Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name").ToList();
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Product product)
        {

            var productCount = db.Products.AsNoTracking().Count();
            productCount += 1;
            string generatedBarcode = $"DC Cat{product.CategoryId} item{productCount}";
            //Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            //byte[] generatedImage = ImageTobyteArray(barcode.Draw(generatedBarcode, 50));

            Product myProduct = new Product()
            {
                CategoryId = product.CategoryId,
                Name = product.Name,
                Price = product.Price,
                InternalImage = product.InternalImage,
                ItemPictureUrl = product.ItemPictureUrl,

            };
            db.Products.Add(myProduct);
            await db.SaveChangesAsync();

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminIndex");
            }
            return RedirectToAction("Index");
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product item = await db.Products.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", item.CategoryId);
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Product product)
        {
            //string name = product.ID.ToString();
            //string cat = product.CategoriesId.ToString();
            //string price = product.Price.ToString();
            //string GeneratedBarcode = "Ad" + name + cat + price;
            //var productCount = db.Products.AsNoTracking().Count();
            //productCount += 1;
            //string generatedBarcode = $"DC Cat{product.CategoryId} item{productCount}";

            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            //byte[] generatedImage = ImageTobyteArray(barcode.Draw(product.Barcode, 50));
            if (ModelState.IsValid)
            {
                Product myProduct = await db.Products.FindAsync(product.ProductId);

                myProduct.CategoryId = product.CategoryId;
                myProduct.Name = product.Name;
                myProduct.Price = product.Price;
                myProduct.InternalImage = product.InternalImage;
                myProduct.ItemPictureUrl = product.ItemPictureUrl;


                //db.Products.Add(myProduct);
                await db.SaveChangesAsync();

                ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            }
            return RedirectToAction("Index");
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product item = await db.Products.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product item = await db.Products.FindAsync(id);
            db.Products.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(int id)
        {
            Product item = await db.Products.FindAsync(id);

            byte[] photoBack = item.InternalImage;

            return File(photoBack, "image/png");
        }

        //public async Task<ActionResult> RenderBarcode(int id)
        //{
        //    Product item = await db.Products.FindAsync(id);

        //    byte[] photoBack = item.BarcodeImage;

        //    return File(photoBack, "image/png");
        //}

        public byte[] ImageTobyteArray(Image imgaeIn)
        {
            using (var ms = new MemoryStream())
            {
                imgaeIn.Save(ms, ImageFormat.Gif);
                return ms.ToArray();
            }
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