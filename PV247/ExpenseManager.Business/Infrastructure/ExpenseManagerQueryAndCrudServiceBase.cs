﻿using System.Collections.Generic;
using AutoMapper;
using ExpenseManager.Business.DTOs;
using ExpenseManager.Database.Infrastructure.Repository;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Business.Infrastructure
{
    /// <summary>
    /// A base class for Query-enabled service, taken from unreleased project of RigantiInfrastructure solution, all credit goes to Tomas Herceg.
    /// </summary>
    /// <typeparam name="TListDTO">The type of the DTO used in the list of records, e.g. in the GridView control.</typeparam>
    public abstract class ExpenseManagerQueryAndCrudServiceBase<TEntity, TKey, TListDTO, TDTO> : ExpenseManagerCrudServiceBase<TEntity, TKey, TDTO> 
        where TEntity : class, IEntity<TKey>, new() 
        where TDTO : ExpenseManagerDTO<TKey>, new()
    {
        /// <summary>
        /// Gets the query object used to populate the list or records.
        /// </summary>
        public IQuery<TListDTO> Query { get; }

        protected ExpenseManagerQueryAndCrudServiceBase(IQuery<TListDTO> query, ExpenseManagerRepository<TEntity, TKey> repository, Mapper expenseManagerMapper, IUnitOfWorkProvider unitOfWorkProvider) : base(repository, expenseManagerMapper, unitOfWorkProvider)
        {
            this.Query = query;
        }

        /// <summary>
        /// Gets the list of the DTOs using the Query object.
        /// </summary>
        public virtual IEnumerable<TListDTO> GetList()
        {
            using (UnitOfWorkProvider.Create())
            {
                return Query.Execute();
            }
        }
    }
}