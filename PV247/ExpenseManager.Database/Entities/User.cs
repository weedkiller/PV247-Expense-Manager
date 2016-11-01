﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpenseManager.Contract.Enums;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Database.Entities
{
    /// <summary>
    /// Class representing user.
    /// </summary>
    public class User : IEntity<int>
    {
        /// <summary>
        /// Id of the user.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Email of the user.
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Account of the user.
        /// </summary>
        [Required]
        public Account Account { get; set; }
        /// <summary>
        /// Access type of the user.
        /// </summary>
        [Required]
        public AccountAccessType AccessType { get; set; }
    }
}