﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Business.Infrastructure;
using ExpenseManager.Business.Infrastructure.CastleWindsor;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Infrastructure.Repository;
using Riganti.Utils.Infrastructure.Core;
using ExpenseManager.Business.Services.Interfaces;
using ExpenseManager.Business.Utilities.BadgeCertification;
using ExpenseManager.Database.DataAccess.FilterInterfaces;
using ExpenseManager.Database.DataAccess.Queries;
using ExpenseManager.Database.Infrastructure.Query;

namespace ExpenseManager.Business.Services.Implementations
{
    /// <summary>
    /// Service handles Badge entity operations
    /// </summary>
    internal class BadgeService : ExpenseManagerQueryAndCrudServiceBase<BadgeModel, Guid, Badge>, IBadgeService
    {
        private readonly IBadgeCertifierResolver _certifierResolver;

        private readonly ExpenseManagerQuery<AccountModel> _accountsQuery;

        private readonly ExpenseManagerRepository<AccountBadgeModel, Guid> _accountBadgeRepository;

        private readonly NotAchievedBadgesQuery _notAchievedBadgesQuery;

        /// <summary>
        /// Included entities
        /// </summary>
        protected override string[] EntityIncludes { get; } =
        {
            nameof(BadgeModel.Accounts)
        };

        /// <summary>
        /// Badge service constructor
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="expenseManagerMapper">Mapper</param>
        /// <param name="unitOfWorkProvider">Unit of work provider</param>
        /// <param name="certifierResolver">Resolves badge certifiers according to badge name</param>
        /// <param name="accountBadgeRepository">Repository for accountBadges</param>
        /// <param name="accountsQuery">Query object for retrieving accounts</param>
        /// <param name="notAchievedBadgesQuery">Query for not achieved badges</param>
        internal BadgeService(ExpenseManagerRepository<BadgeModel, Guid> repository, Mapper expenseManagerMapper, IUnitOfWorkProvider unitOfWorkProvider, ExpenseManagerQuery<AccountModel> accountsQuery, IBadgeCertifierResolver certifierResolver, ExpenseManagerRepository<AccountBadgeModel, Guid> accountBadgeRepository, ExpenseManagerQuery<BadgeModel> notAchievedBadgesQuery) 
            : base(BusinessLayerDIManager.Resolve<ExpenseManagerQuery<BadgeModel>>("ListBadgesQuery"), repository, expenseManagerMapper, unitOfWorkProvider)
        {
            _accountsQuery = accountsQuery;
            _certifierResolver = certifierResolver;
            _accountBadgeRepository = accountBadgeRepository;
            _notAchievedBadgesQuery = notAchievedBadgesQuery as NotAchievedBadgesQuery;
        }

        /// <summary>
        /// Creates new Badge object in database
        /// </summary>
        public void CreateBadge(Badge badge)
        {
            using (var unitOfWork = UnitOfWorkProvider.Create())
            {
                Save(badge);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Updates existing badge in database
        /// </summary>
        /// <param name="badgeEdited"></param>
        public void UpdateBadge(Badge badgeEdited)
        {
            using (var unitOfWork = UnitOfWorkProvider.Create())
            {
                Save(badgeEdited);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Deletes badge cpecified by id
        /// </summary>
        /// <param name="badgeId"></param>
        public void DeleteBadge(Guid badgeId)
        {
            using (var unitOfWork = UnitOfWorkProvider.Create())
            {
                Delete(badgeId);
                unitOfWork.Commit();
            }           
        }

        /// <summary>
        /// Get specific badge by unique id
        /// </summary>
        /// <param name="badgeId"></param>
        /// <returns></returns>
        public Badge GetBadge(Guid badgeId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return GetDetail(badgeId);
            }           
        }

        /// <summary>
        /// Lists filtered badges
        /// </summary>
        /// <param name="filters">Filters badges</param>
        /// <param name="pageAndOrder"></param>
        /// <returns></returns>
        public List<Badge> ListBadges(IEnumerable<IFilter<BadgeModel>> filters, IPageAndOrderable<BadgeModel> pageAndOrder)
        {
            Query.Filters = filters;
            Query.PageAndOrderModelFilterModel = pageAndOrder;
            using (UnitOfWorkProvider.Create())
            {
                return GetList().ToList();
            }           
        }

        /// <summary>
        /// Lists all not achieved badges for given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<Badge> ListNotAchievedBadges(Guid accountId)
        {
            using (UnitOfWorkProvider.Create())
            {
                _notAchievedBadgesQuery.AccountId = accountId;
                return ExpenseManagerMapper.Map<List<Badge>>(_notAchievedBadgesQuery.Execute());
            }
        }

        /// <summary>
        /// Check all accounts if they deserve to assign badges
        /// </summary>
        public void CheckBadgesRequirements()
        {
            using (var unitOfWork = UnitOfWorkProvider.Create())
            {
                var allAccounts = _accountsQuery.Execute();
                var allBadges = Query.Execute();
                foreach (var account in allAccounts)
                {
                    foreach (var badge in allBadges)
                    {
                        var assignBadge = _certifierResolver.ResolveBadgeCertifier(badge.Name)
                            ?.CanAssignBadge(account) ?? false;

                        if (assignBadge)
                        {
                            _accountBadgeRepository.Insert(new AccountBadgeModel
                            {
                                Account = account,
                                Badge = badge,
                                Achieved = DateTime.Now
                            });
                        }
                    }
                }
                unitOfWork.Commit();
            }
        }
    }
}
