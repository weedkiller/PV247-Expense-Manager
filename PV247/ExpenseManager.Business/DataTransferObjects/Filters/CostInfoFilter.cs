﻿using System;

namespace ExpenseManager.Business.DataTransferObjects.Filters
{
    /// <summary>
    /// Filter userd in queries in order to get cost infos with specifies parameters
    /// </summary>
    public class CostInfoFilter : FilterBase
    {
        /// <summary>
        /// Filter of income if false, do not filter if is null
        /// </summary>
        public bool? IsIncome { get; set; }
        /// <summary>
        /// Left edge of money range
        /// </summary>
        public int? MoneyFrom { get; set; }
        /// <summary>
        /// Right edge of money range
        /// </summary>
        public int? MoneyTo { get; set; }
        /// <summary>
        /// Account id to be filtered with
        /// </summary>
        public int? AccountId { get; set; }
        /// <summary>
        /// Account name to be filtered with
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// Determines if Equals() or Contains() should be used when matching string parameters
        /// </summary>
        public bool DoExactMatch { get; set; }
        /// <summary>
        /// Left edge of created range
        /// </summary>
        public DateTime? CreatedFrom { get; set; }
        /// <summary>
        /// Right edge of created range
        /// </summary>
        public DateTime? CreatedTo { get; set; }
        /// <summary>
        /// Type id to be filtered with
        /// </summary>
        public int? TypeId { get; set; }
        /// <summary>
        /// Type name to be 
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Filters out periodic costs, does not filter if null
        /// </summary>
        public bool? IsPeriodic { get; set; }
    }
}
