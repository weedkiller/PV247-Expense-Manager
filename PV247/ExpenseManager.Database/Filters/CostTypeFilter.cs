﻿namespace ExpenseManager.Database.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CostTypeFilter : FilterBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DoExactMatch { get; set; }
    }
}
