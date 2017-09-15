using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RolesAdminController : BaseController
    {
        AjaoOkoDb _db;

        public RolesAdminController()
        {
            _db = new AjaoOkoDb();
        }



        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var Roles = _db.Roles.ToList();
            return View(Roles);
        }

        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            _db.Roles.Add(Role);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
