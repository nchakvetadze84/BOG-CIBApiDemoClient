using System;
using System.Collections.Generic;

namespace CIBApiDemoClient.Model
{
    /// <summary>
    /// api.model.balance.sheet
    /// </summary>
    public class BalanceSheet
    {
        /// <summary>
        /// api.property.start.date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// api.property.end.date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// api.property.balance.sheet.entries
        /// </summary>
        public List<BalanceSheetEntry> BalanceSheetEntries { get; set; }
    }

    /// <summary>
    /// api.model.balance.sheet.entry
    /// </summary>
    public class BalanceSheetEntry
    {
        /// <summary>
        /// api.property.account
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// api.property.account.description
        /// </summary>
        public string AccountDescription { get; set; }

        /// <summary>
        /// api.property.opening.debit
        /// </summary>
        public decimal? OpeningDebit { get; set; }

        /// <summary>
        /// api.property.opening.credit
        /// </summary>
        public decimal? OpeningCredit { get; set; }

        /// <summary>
        /// api.property.debit.turnover
        /// </summary>
        public decimal? DebitTurnover { get; set; }

        /// <summary>
        /// api.property.credit.turnover
        /// </summary>
        public decimal? CreditTunover { get; set; }

        /// <summary>
        /// api.property.net.turnover
        /// </summary>
        public decimal? NetTurnover { get; set; }

        /// <summary>
        /// api.property.closing.debit
        /// </summary>
        public decimal? ClosingDebit { get; set; }

        /// <summary>
        /// api.property.closing.credit
        /// </summary>
        public decimal? ClosingCredit { get; set; }

        /// <summary>
        /// api.property.opening.quantity.debit
        /// </summary>
        public decimal? OpeningQuantityDebit { get; set; }

        /// <summary>
        /// api.property.opening.quantity.credit
        /// </summary>
        public decimal? OpeningQuantityCredit { get; set; }

        /// <summary>
        /// api.property.quantity.debit
        /// </summary>
        public decimal? QuantityDebit { get; set; }

        /// <summary>
        /// api.property.quantity.credit
        /// </summary>
        public decimal? QuantityCredit { get; set; }

        /// <summary>
        /// api.property.quantity.net.turnover
        /// </summary>
        public decimal? QuantityNetTurnover { get; set; }

        /// <summary>
        /// api.property.value.calculated
        /// </summary>
        public bool ValueCalculated { get; set; }

        /// <summary>
        /// api.property.identity.code
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        /// api.property.final.debit 
        /// </summary>
        public decimal? QuantityFinalDebit { get; set; }

        /// <summary>
        /// api.property.final.credit
        /// </summary>
        public decimal? QuantityFinalCredit { get; set; }
    }
}