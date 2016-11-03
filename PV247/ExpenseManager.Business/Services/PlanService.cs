﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ExpenseManager.Business.DTOs;
using ExpenseManager.Business.Infrastructure;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Infrastructure.Repository;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Business.Services
{
    public class PlanAndCrudService : ExpenseManagerQueryAndCrudServiceBase<Plan, int, IList<PlanDTO>, PlanDTO>
    {
        public PlanAndCrudService(IQuery<IList<PlanDTO>> query, ExpenseManagerRepository<Plan, int> repository, Mapper expenseManagerMapper, IUnitOfWorkProvider unitOfWorkProvider)
            : base(query, repository, expenseManagerMapper, unitOfWorkProvider) { }

        public void CreatePlan(PlanDTO planDTO)
        {
            Save(planDTO);
        }

        // TODO add more functionality


        protected override Expression<Func<PlanDTO, object>>[] EntityIncludes => new Expression<Func<PlanDTO, object>>[]
        {
            
        };
    }
}
