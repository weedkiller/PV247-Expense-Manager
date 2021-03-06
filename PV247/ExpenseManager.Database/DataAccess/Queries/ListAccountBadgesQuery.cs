﻿using System.Linq;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Infrastructure.Query;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Database.DataAccess.Queries
{
    /// <summary>
    /// Implementation of Query for account badges.
    /// </summary>
    internal class ListAccountBadgesQuery : ExpenseManagerQuery<AccountBadgeModel>
    {
        /// <summary>
        /// Create query.
        /// </summary>
        /// <param name="provider">unitOfWork provider</param>
        internal ListAccountBadgesQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Return IQueryable.
        /// </summary>
        /// <returns>IQueryable</returns>
        protected override IQueryable<AccountBadgeModel> GetQueryable()
        {
            return ApplyFilters(Context.AccountBadges.Include(nameof(AccountBadgeModel.Account)).Include(nameof(AccountBadgeModel.Badge)));
        }
    }
}
