﻿using System.Linq;
using ExpenseManager.Database.DataAccess.FilterInterfaces;
using ExpenseManager.Database.Entities;

namespace ExpenseManager.Business.DataTransferObjects.Filters.Users
{
    /// <summary>
    /// Filters by user name
    /// </summary>
    public class UsersByAccountName : IFilter<UserModel>
    {
        /// <summary>
        /// Specifies account name to filter with
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Filters by user name
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public IQueryable<UserModel> FilterQuery(IQueryable<UserModel> queryable)
        {
            return queryable.Where(user => user.Account.Name.Contains(AccountName));
        }
    }
}
