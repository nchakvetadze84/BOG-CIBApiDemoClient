using System;

namespace CIBApiDemoClient.Model
{
    public class AccountBalance
    {
        public double AvailableBalance { get; set; }
        public double CurrentBalance { get; set; }
    }

    public class TodayActivityDetail
    {
        public long Id { get; set; }
        public long DocKey { get; set; }
        public string DocNo { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string EntryType { get; set; }
        public string EntryComment { get; set; }
        public string EntryCommentEn { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountBase { get; set; }
        public string PayerName { get; set; }
        public string PayerInn { get;  set; }
        public AccountDetails Sender { get; set; }
        public AccountDetails Beneficiary { get; set; }
    }
}