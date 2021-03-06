﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpenseManager.Database.DataAccess.FilterInterfaces;
using ExpenseManager.Database.Infrastructure.UnitOfWork;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Database.Infrastructure.Query
{
    /// <summary>
    /// A base implementation of query object in Entity Framework.
    /// </summary>
    public abstract class ExpenseManagerQuery<TResult> : QueryBase<TResult>
    {
        private readonly IUnitOfWorkProvider _provider;

        /// <summary>
        /// Filters used to determine parameters of query
        /// </summary>
        public IEnumerable<IFilter<TResult>> Filters;

        /// <summary>
        /// Filter used for paging and filtering
        /// </summary>
        public IPageAndOrderable<TResult> PageAndOrderModelFilterModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseManagerQuery{TResult}"/> class.
        /// </summary>
        protected ExpenseManagerQuery(IUnitOfWorkProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Gets the <see cref="DbContext"/>.
        /// </summary>
        internal ExpenseDbContext Context => (ExpenseDbContext)ExpenseManagerUnitOfWork.TryGetDbContext(_provider);

        /// <summary>
        /// Return IQueryable.
        /// </summary>
        /// <returns>IQueryable</returns>
        protected IQueryable<TResult> ApplyFilters(IQueryable<TResult> queryable)
        {
            queryable = Filters == null ? queryable : Filters.Aggregate(queryable, (current, filter) => filter == null ? current : filter.FilterQuery(current));
            return PageAndOrderModelFilterModel == null ? queryable : PageAndOrderModelFilterModel.FilterQuery(queryable);
        }
    }
}
