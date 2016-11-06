﻿namespace ExpenseManager.Business.DataTransferObjects.Filters
{
    /// <summary>
    /// Filter userd in queries in order to get badges with specifies parameters
    /// </summary>
    public class BadgeFilter : FilterBase
    {
        /// <summary>
        /// Description to be filtered with
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Determines if Equals() or Contains() should be used when matching string parameters
        /// </summary>
        public bool DoExactMatch { get; set; }
    }
}
