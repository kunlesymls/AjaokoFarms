namespace AdunbiKiddies.Models
{
    public class PopUp
    {

        public enum Status
        {
            Add = 1, Remove
        }
        public enum Unit
        {
            Kg = 1, Litres, Bag, Crate, Basket, Tubbers, Mudu, Cup, Bottle, Tonnnes
        }

        public enum Withdrawal
        {
            Processing = 0, Successful = 1, Declined
        }
        public enum BankName
        {
            Access = 1,
            Citibank,
            Diamond,
            DynamicStandard,
            Ecobank,
            Fidelity,
            FirstBank,
            FCMB,
            GTB,
            Heritage,
            Keystone,
            Providus,
            Skye,
            StanbicIBTC,
            StandardChartered,
            Sterling,
            Suntrust,
            Union,
            UBA,
            Unity,
            Wema,
            Zenith,
        }
    }
}