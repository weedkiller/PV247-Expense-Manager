﻿using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Presentation.Models.Achievement;
using ExpenseManager.Presentation.Models.ViewComponent;

namespace ExpenseManager.Presentation.Infrastructure.Mapping
{
    /// <summary>
    /// Class for managing mapping
    /// </summary>
    public class BussinessToViewModelMapping : Profile
    {
        /// <summary>
        /// Constructor where mapping is done
        /// </summary>
        public BussinessToViewModelMapping()
        {
            CreateMap<CostInfo, Models.Expense.IndexViewModel>()
                .ReverseMap();

            CreateMap<CostInfo, Models.PermanentExpense.IndexPermanentExpenseViewModel>()
                .ReverseMap();

            CreateMap<CostInfo, Models.Expense.CreateViewModel>()
                .ReverseMap();

            CreateMap<CostInfo, Models.PermanentExpense.CreatePermanentExpenseViewModel>()
                .ReverseMap();

            CreateMap<CostType, Models.CostType.CategoryViewModel>()
                .ReverseMap();

            CreateMap<CostType, Models.CostType.CreateViewModel>()
                .ReverseMap();

            CreateMap<User, Models.User.IndexViewModel>()
                .ReverseMap();

            CreateMap<Plan, Models.Plan.PlanViewModel>()
                .ReverseMap();

            CreateMap<Plan, Models.Plan.CreateViewModel>()
                .ReverseMap();

            CreateMap<Badge, BadgeViewModel>()
                .ReverseMap();

            CreateMap<AccountBadge, AccountBadgeViewModel>()
                .ReverseMap();

            CreateMap<DayTotalBalance, DayTotalBalanceViewModel>()
                .ReverseMap();
        }
    }
}