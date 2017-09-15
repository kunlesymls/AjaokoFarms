namespace AdunbiKiddies.Models
{
    public class BusinessAddress
    {
        public int BusinessAddressId { get; set; }
        public string MerchantId { get; set; }
        public string AddressType { get; set; }
        public string BuildingNo { get; set; }
        public string StreetName { get; set; }
        public string TownName { get; set; }
        public string StateName { get; set; }
        public virtual Merchant Merchant { get; set; }
    }
}