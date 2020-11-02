using System;
using System.Collections.Generic;

namespace CIBApiDemoClient.Model
{
    public class DocumentStatus
    {
        public Guid UniqueId { get; set; }

        public long? UniqueKey { get; set; }

        public string Status { get; set; }

        public string BulkLineStatus { get; set; }

        public int? RejectCode { get; set; }

        public int? ResultCode { get; set; }

        public decimal? Match { get; set; }
    }

    public class BulkPaymentStatus
    {
        public string Status { get; set; }

        public List<DocumentStatus> DocumentStatuses { get; set; }
    }
}