﻿namespace ExpenseManager.Database.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class BadgeFilter : FilterBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DoExactMatch { get; set; }
    }
}
