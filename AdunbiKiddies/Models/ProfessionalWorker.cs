using System.Collections.Generic;

namespace OpenOrderFramework.Models
{
    public class ProfessionalWorker
    {
        public string ProfessionalWorkerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public virtual ICollection<ProfessionalPayment> ProfessionalPayments { get; set; }
    }

    public class ProfessionalPayment
    {
        public int ProfessionalPaymentId { get; set; }
        public string ProfessionalWorkerId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public virtual ProfessionalWorker ProfessionalWorker { get; set; }
    }
}