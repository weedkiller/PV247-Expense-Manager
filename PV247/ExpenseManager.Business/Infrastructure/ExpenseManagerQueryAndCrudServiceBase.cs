﻿using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Database.Filters;
using ExpenseManager.Database.Infrastructure.Query;
using ExpenseManager.Database.Infrastructure.Repository;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Business.Infrastructure
{
    /// <summary>
    /// A base class for Query-enabled service, taken from unreleased project of RigantiInfrastructure solution, all credit goes to Tomas Herceg.
    /// </summary>
    /// <typeparam name="TList">The type of the  used in the list of records, e.g. in the GridView control.</typeparam>
    public abstract class ExpenseManagerQueryAndCrudServiceBase<TEntity, TKey, TQuery, T, TFilter> : ExpenseManagerCrudServiceBase<TEntity, TKey, T> 
        where TEntity : class, IEntity<TKey>, new() 
        where T : BusinessObject<TKey>, new()
        where TQuery : ExpenseManagerQuery<TEntity, TFilter>
        where TFilter : FilterModelBase, new ()
    {
        /// <summary>
        /// Gets the query object used to populate the list or records.
        /// </summary>
        public TQuery Query { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="repository"></param>
        /// <param name="expenseManagerMapper"></param>
        /// <param name="unitOfWorkProvider"></param>
        protected ExpenseManagerQueryAndCrudServiceBase(TQuery query, ExpenseManagerRepository<TEntity, TKey> repository, Mapper expenseManagerMapper, IUnitOfWorkProvider unitOfWorkProvider) : base(repository, expenseManagerMapper, unitOfWorkProvider)
        {
            this.Query = query;
        }

        /// <summary>
        /// Gets the list of the s using the Query object.
        /// </summary>
        public virtual IList<T> GetList()
        {
            using (UnitOfWorkProvider.Create())
            {
                return ExpenseManagerMapper.Map<IList<TEntity>, IList<T>>(Query.Execute());
            }
        }
    }
}