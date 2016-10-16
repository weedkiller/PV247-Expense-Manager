﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    /// <summary>
    /// Represents type of the cost information.
    /// </summary>
    public class CostType
    {
        /// <summary>
        /// Id of the cost type.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of this type.
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }
        /// <summary>
        /// All costs of this type.
        /// </summary>
        public virtual List<CostInfo> CostInfoList { get; set; }
    }
}
