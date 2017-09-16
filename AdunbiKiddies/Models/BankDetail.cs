namespace AdunbiKiddies.Models
{
    public class BankDetail
    {
        public int BankDetailId { get; set; }
        public string UserId { get; set; }
        public string AccountName { get; set; }
        public int AccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public bool IsPrimary { get; set; }
    }
}