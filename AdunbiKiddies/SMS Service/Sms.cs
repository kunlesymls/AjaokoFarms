using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.SMS_Service
{
    public class Sms
    {
        public string Recipient { get; set; }

        public string Sender { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }

    public class EmailVm
    {
        public string Destination { get; set; }

        public string Subject { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}