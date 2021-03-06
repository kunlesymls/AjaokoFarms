﻿using AdunbiKiddies.Models;
using AdunbiKiddies.Models.Repository;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class HomeController : BaseController
    {
        //CategoryRepo CatRepo = new CategoryRepo();
        //StoreSectionRepo storeRepo = new StoreSectionRepo();
        ProductRepo ProductRepo = new ProductRepo();

        //For view testing 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Landing()
        {
            return View();
        }
        public ActionResult SuperAdminDashBoard()
        {
            return View();

        }
        public ActionResult ProfessionalDashBoard()
        {
            return View();
        }
        public ActionResult MerchantDashBoard()
        {
            return View();
        }

        public ActionResult CategoryItems(int id)
        {
            if (ProductRepo.GetProductByCat(id).Count > 0)
            {
                List<Product> model = ProductRepo.GetProductByCat(id);
                return View(model);
            }
            else
            {
                ViewBag.Mesage = "No matching result";
                return View();
            }
            //return RedirectToAction("Index");
        }
        public ActionResult Item(int id)
        {
            if (ProductRepo.GetProductByCat(id).Count > 0)
            {
                List<Product> model = ProductRepo.GetProductByCat(id);
                return View(model);
            }
            ViewBag.Mesage = "No matching result";
            return View();
            //return RedirectToAction("Index");
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Barcode()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}