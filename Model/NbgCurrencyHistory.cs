using System;

namespace CIBApiDemoClient.Model
{
    public class NbgCurrencyHistory
    {
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
    }
}