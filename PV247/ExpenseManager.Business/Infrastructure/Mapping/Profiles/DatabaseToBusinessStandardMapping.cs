﻿using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Business.DataTransferObjects.Filters;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Filters;

namespace ExpenseManager.Business.Infrastructure.Mapping.Profiles
{
    /// <summary>
    /// Standard mapping profile.
    /// </summary>
    public class DatabaseToBusinessStandardMapping : Profile
    {
        /// <summary>
        /// Creates mapping.
        /// </summary>
        public DatabaseToBusinessStandardMapping()
        {
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.AccountId, opts => opts.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.AccountName, opts => opts.MapFrom(src => src.Account.Name))
                .ReverseMap();

            CreateMap<PlanModel,Plan>()
                .ForMember(dest => dest.AccountId, opts => opts.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.AccountName, opts => opts.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.PlannedTypeId, opts => opts.MapFrom(src => src.PlannedType.Id))
                .ForMember(dest => dest.PlannedTypeName, opts => opts.MapFrom(src => src.PlannedType.Name))
                .ReverseMap();

            CreateMap<CostTypeModel, CostType>()
                .ReverseMap();

            CreateMap<CostInfoModel, CostInfo>()
                .ForMember(dest => dest.AccountId, opts => opts.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.AccountName, opts => opts.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => src.Type.Id))
                .ForMember(dest => dest.TypeName, opts => opts.MapFrom(src => src.Type.Name))
                .ReverseMap();

            CreateMap<BadgeModel, Badge>()
                .ReverseMap();

            CreateMap<AccountBadgeModel, AccountBadge>()
                .ForMember(dest => dest.AccountId, opts => opts.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.AccountName, opts => opts.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.BadgeId, opts => opts.MapFrom(src => src.Badge.Id))
                .ForMember(dest => dest.BadgeDescription, opts => opts.MapFrom(src => src.Badge.Description))
                .ForMember(dest => dest.BadgeImgUri, opts => opts.MapFrom(src => src.Badge.BadgeImgUri))
                .ReverseMap();

            CreateMap<AccountModel, Account>()
               .ReverseMap();

            /*      CreateMap<AccountAccessType, AccountAccessTypeModel>()
                    .ReverseMap();

                  CreateMap<PlanType, PlanTypeModel>()
                   .ReverseMap();*/

            CreateMap<AccountBadgeFilter, AccountBadgeModelFilter>()
             .ReverseMap();

            CreateMap<AccountFilter, AccountModelFilter>()
             .ReverseMap();

            CreateMap<BadgeFilter, BadgeModelFilter>()
             .ReverseMap();

            CreateMap<CostInfoFilter, CostInfoModelFilter>()
             .ReverseMap();

            CreateMap<CostTypeFilter, CostTypeModelFilter>()
             .ReverseMap();

            CreateMap<PlanFilter, PlanModelFilter>()
             .ReverseMap();

            CreateMap<UserFilter, UserModelFilter>()
             .ReverseMap();

        }
    }
}
