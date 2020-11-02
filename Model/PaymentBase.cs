using System;

namespace CIBApiDemoClient.Model
{
    public class PaymentBase
    {
        public PaymentBase()
        {
            Amount = 1;
            UniqueId = Guid.NewGuid();
            ValueDate = DateTime.Today;
        }

        public Guid UniqueId { get; set; }
        public decimal Amount { get; set; }
        public virtual string Currency { get; set; }
        public string DocumentNo { get; set; }
        public string SourceAccountNumber { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public DateTime ValueDate { get; set; }
        public bool CheckInn { get; set; }
    }

    public class DomesticPayment : PaymentBase
    {
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryInn { get; set; }
        public string BeneficiaryName { get; set; }
        public override string Currency
        {
            get { return "GEL"; }
            set { }
        }
        public string AdditionalInformation { get; set; }
        public string Nomination { get; set; }
        public string DispatchType { get; set; }
        public string PayerInn { get; set; }
        public string PayerName { get; set; }
    }

    public class ForeignPayment : PaymentBase
    {
        public string Charges { get; set; }
        public string ComissionAccountNumber { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryInn { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string BeneficiaryName { get; set; }
        public string IntermediaryBankCode { get; set; }
        public string IntermediaryBankName { get; set; }
        public string AdditionalInformation { get; set; }
        public string PaymentDetail { get; set; }
        public string BeneficiaryRegistrationCountryCode { get; set; }
        public string BeneficiaryActualCountryCode { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCity { get; set; }
        public string RegReportingValue { get; set; }
    }

    public class DocumentKey
    {
        public Guid UniqueId { get; set; }
        public long UniqueKey { get; set; }
        public int? ResultCode { get; set; }
    }

    public class ConversionPayment
    {
        public ConversionPayment()
        {
            Amount = 1;
            UniqueId = Guid.NewGuid();
        }

        public Guid UniqueId { get; set; }
        public string DocumentNo { get; set; }
        public string SourceAccountNumber { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string DestinationCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public string AdditionalInfo { get; set; }
    }
}