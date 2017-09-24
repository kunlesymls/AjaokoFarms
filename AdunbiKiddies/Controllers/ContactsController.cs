using AdunbiKiddies.Models;
using AdunbiKiddies.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    [Authorize]
    public class ContactsController : BaseController
    {
        public ContactRepo contactRepo = new ContactRepo();
        // GET: Contacts
        public ActionResult Index()
        {
            return View();
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [AllowAnonymous]
        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult _Contact(Contact contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(contact);
                }

                contactRepo.Add(contact);
                contactRepo.SaveChanges();
                TempData["Contact"] = "Hello" + "" + contact.Name + "" + "your Comment has been recieved";

                return PartialView();

            }
            catch
            {
                TempData["Contact"] = "Hello" + "" + contact.Name + "" + "your Comment didn't come by , Please try again. Thanks";
                return PartialView("_Contact");
            }
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
