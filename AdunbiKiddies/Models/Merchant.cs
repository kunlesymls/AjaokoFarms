using OpenOrderFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace AdunbiKiddies.Models
{
    public class Merchant
    {
        public string MerchantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PreferedStoreName { get; set; }
        public bool IsReffered { get; set; }
        public string BusinessEntry { get; set; }
        public string IdCardName { get; set; }
        public byte[] Idcard { get; set; }
        public string RegistrationNo { get; set; }

        public bool IsVerified { get; set; }
        [Display(Name = "ID Card")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Idcard = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

        public virtual PartnerShipAgreement PartnerShipAgreement { get; set; }
        public virtual ICollection<BusinessRegistration> BusinessRegistrations { get; set; }
        public virtual ICollection<BusinessAddress> BusinessAddresses { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}