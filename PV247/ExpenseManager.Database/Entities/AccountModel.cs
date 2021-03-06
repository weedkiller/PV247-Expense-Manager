﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Database.Entities
{
    /// <summary>
    /// Represents account of the user.
    /// </summary>
    public class AccountModel : IEntity<Guid>
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the account.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// List of users that have access to this account.
        /// </summary>
        public virtual List<UserModel> Users { get; set; }

        /// <summary>
        /// User's costs.
        /// </summary>
        public virtual List<CostInfoModel> Costs { get; set; }

        /// <summary>
        /// All plans of the user.
        /// </summary>
        public virtual List<PlanModel> Plans { get; set; }

        /// <summary>
        /// All badges of the user.
        /// </summary>
        public virtual List<AccountBadgeModel> Badges { get; set; }

        /// <summary>
        /// Cost types of the user.
        /// </summary>
        public virtual List<CostTypeModel> CostTypes { get; set; }
    }
}
