namespace CIBApiDemoClient.Model
{
    public class AccountDetails
    {
        public string Name { get; set; }
        public string Inn { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Name, Inn, AccountNumber, BankCode, BankName);
        }
    }
}