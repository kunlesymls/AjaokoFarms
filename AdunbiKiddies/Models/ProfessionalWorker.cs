using AdunbiKiddies.Models;
using System;
using System.Collections.Generic;

namespace OpenOrderFramework.Models
{
    public class ProfessionalWorker : Person
    {
        public string ProfessionalWorkerId { get; set; }
        public string HighestQualification { get; set; }
        public virtual ICollection<ProfessionalPayment> ProfessionalPayments { get; set; }
        public virtual ICollection<ProfessionalAddress> ProfessionalAddresses { get; set; }
        public virtual ICollection<ProfessionalDocument> ProfessionalDocuments { get; set; }
    }

    public class ProfessionalPayment
    {
        public int ProfessionalPaymentId { get; set; }
        public string ProfessionalWorkerId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public bool IsPayed { get; set; }

        public string Status
        {
            get
            {
                if (IsPayed)
                {
                    return "Payment Confirmed";
                }
                return "Payment Not Received";
            }
            set { }
        }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public virtual ProfessionalWorker ProfessionalWorker { get; set; }
    }

    public class ProfessionalQualification
    {
        public int ProfessionalQualificationId { get; set; }
        public string ProfessionalWorkerId { get; set; }

        public string WorkingExperince { get; set; }
        public string ProjectDescription { get; set; }
        public string ShortBio { get; set; }
        public virtual ProfessionalWorker ProfessionalWorker { get; set; }

    }

    public class ProfessionalDocument
    {
        public int ProfessionalDocumentId { get; set; }
        public string ProfessionalWorkerId { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public virtual ProfessionalWorker ProfessionalWorker { get; set; }
    }
}