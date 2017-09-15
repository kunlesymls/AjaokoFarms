using AdunbiKiddies.BusinessLogic;
using AdunbiKiddies.Models;
using AdunbiKiddies.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace AdunbiKiddies.Controllers
{
    public class BaseController : Controller
    {
        public readonly AjaoOkoDb _db;
        public readonly QueryManager _query;

        public string merchantId;

        public BaseController()
        {
            _db = new AjaoOkoDb();
            _query = new QueryManager();
            merchantId = _query.GetId();
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var user = User.Identity.GetUserId();
            var merchant = _db.Merchants.AsNoTracking().FirstOrDefault(x => x.MerchantId.Equals(user));
            if (merchant != null) merchantId = merchant.MerchantId;

            // var model = filterContext.Controller.ViewData.Model as BaseViewModel;
            var model = new BaseViewModel();

            if (merchant != null)
            {
                model.Alias = merchant.FullName;
                model.PerfereedName = merchant.PreferedStoreName;
                model.MerchantId = merchant.MerchantId;
                //ViewBag.ImageId = school.SchoolId;
            }
            else
            {
                model.Alias = "AJAOKO";
                model.PerfereedName = "AJAOKO";
                model.MerchantId = "AJAOKO";
                model.Color = "";
            }
            ViewBag.LayoutViewModel = model;



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