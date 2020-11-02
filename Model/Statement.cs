using System;
using System.Collections.Generic;

namespace CIBApiDemoClient.Model
{
    public class Statement
    {
        public long Id { get; set; }
        public int Count { get; set; }
        public List<StatementDetail> Records { get; set; }
    }

    public class StatementDetail
    {
        public decimal? Rate { get; set; }
        public DateTime? EntryDate { get; set; }

        public string EntryDocumentNumber { get; set; }
        public string EntryAccountNumber { get; set; }
        public decimal? EntryAmountDebit { get; set; }
        public decimal? EntryAmountDebitBase { get; set; }
        public decimal? EntryAmountCredit { get; set; }
        public decimal? EntryAmountCreditBase { get; set; }
        public decimal? EntryAmountBase { get; set; }
        public decimal? EntryAmount { get; set; }
        public string EntryComment { get; set; }
        public string EntryDepartment { get; set; }
        public string EntryAccountPoint { get; set; }

        public string DocumentProductGroup { get; set; }
        public DateTime? DocumentValueDate { get; set; }

        public AccountDetails SenderDetails { get; set; }
        public AccountDetails BeneficiaryDetails { get; set; }

        public string DocumentTreasuryCode { get; set; }

        public string DocumentNomination { get; set; }
        public string DocumentInformation { get; set; }

        public decimal? DocumentSourceAmount { get; set; }

        public string DocumentSourceCurrency { get; set; }

        public decimal? DocumentDestinationAmount { get; set; }

        public string DocumentDestinationCurrency { get; set; }

        public DateTime? DocumentReceiveDate { get; set; }

        public string DocumentBranch { get; set; }

        public DateTime? DocumentActualDate { get; set; }
        public DateTime? DocumentExpiryDate { get; set; }

        public decimal? DocumentRateLimit { get; set; }
        public decimal? DocumentRate { get; set; }
        public decimal? DocumentRegistrationRate { get; set; }

        public string DocumentSenderInstitution { get; set; }
        public string DocumentIntermediaryInstitution { get; set; }
        public string DocumentBeneficiaryInstitution { get; set; }

        public string DocumentPayee { get; set; }

        public string DocumentCorrespondentAccountNumber { get; set; }
        public string DocumentCorrespondentBankCode { get; set; }

        public string DocumentCorrespondentBankName { get; set; }
    }

    public class DailySummary
    {
        public decimal Balance { get; set; }
        public decimal BalanceBase { get; set; }
        public decimal CreditSum { get; set; }
        public decimal DebitSum { get; set; }
        public decimal Rate { get; set; }
        public int EntryCount { get; set; }
        public DateTime? Date { get; set; }
    }

    public class GlobalSummary
    {
        public string AccountNumber { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public decimal InAmount { get; set; }
        public decimal InAmountBase { get; set; }
        public decimal InRate { get; set; }
        public decimal OutAmount { get; set; }
        public decimal OutAmountBase { get; set; }
        public decimal OutRate { get; set; }
        public decimal CreditSum { get; set; }
        public decimal DebitSum { get; set; }
    }

    public class StatementSummary
    {
        public StatementSummary()
        {
            DailySummaries = new List<DailySummary>();
        }

        public GlobalSummary GlobalSummary { get; set; }

        public List<DailySummary> DailySummaries { get; set; }
    }
}