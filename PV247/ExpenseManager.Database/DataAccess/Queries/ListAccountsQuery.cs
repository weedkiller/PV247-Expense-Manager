﻿using System;
using System.Linq;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Filters;
using ExpenseManager.Database.Infrastructure.Query;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Database.DataAccess.Queries
{
    /// <summary>
    /// Implementation of Query for accounts.
    /// </summary>
    public class ListAccountsQuery : ExpenseManagerQuery<AccountModel, AccountModelFilter>
    {
        /// <summary>
        /// Create query.
        /// </summary>
        /// <param name="provider">UoW provider</param>
        public ListAccountsQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }
        /// <summary>
        /// Account filter
        /// </summary>
        public override AccountModelFilter Filter { get; set; }
        /// <summary>
        /// Return IQueryable.
        /// </summary>
        /// <returns>IQueryable</returns>
        protected override IQueryable<AccountModel> GetQueryable()
        {
            IQueryable<AccountModel> accounts = Context.Accounts;

            if (Filter == null)
            {
                return accounts;
            }
            if (!string.IsNullOrEmpty(Filter.Name))
            {
                accounts = Filter.DoExactMatch ? accounts.Where(account => account.Name.Equals(Filter.Name)) : accounts.Where(account => account.Name.Contains(Filter.Name));
            }
            if (Filter.OrderByDesc == null || string.IsNullOrEmpty(Filter.OrderByPropertyName))
            {
                return accounts;
            }
            System.Reflection.PropertyInfo prop = typeof(AccountModel).GetProperty(Filter.OrderByPropertyName);
            if (prop == null)
            {
                return accounts;
            }
            accounts = Filter.OrderByDesc.Value ? accounts.OrderByDescending(x => prop.GetValue(x, null)) : accounts.OrderBy(x => prop.GetValue(x, null));
            if (Filter.PageNumber != null)
            {
                accounts = accounts.Skip(Math.Max(0, Filter.PageNumber.Value - 1) * Filter.PageSize);
            }
            return accounts.Take(Filter.PageSize);
        }
    }
}
