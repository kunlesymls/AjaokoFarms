using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdunbiKiddies.SMS_Service;

namespace AdunbiKiddies.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly SmsServiceTemp _smsService;

        public NotificationsController()
        {
            _smsService = new SmsServiceTemp();
        }

        // GET: Notifications
        public ActionResult Index()
        {
            SendMessage("Just testing", "07036927669");
            return RedirectToAction("Index", "Home");
        }

        private void SendMessage(string body, string phoneNumber)
        {
            Sms sms = new Sms()
            {
                Sender = "De Choice",
                Message = body,
                Recipient = phoneNumber
            };
            string response = _smsService.Send(sms);
        }
    }
}