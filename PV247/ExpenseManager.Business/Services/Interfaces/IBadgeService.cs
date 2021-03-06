﻿using System;
using System.Collections.Generic;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Database.DataAccess.FilterInterfaces;
using ExpenseManager.Database.Entities;

namespace ExpenseManager.Business.Services.Interfaces
{
    /// <summary>
    /// Service handles Badge entity operations
    /// </summary>
    internal interface IBadgeService : IService
    {
        /// <summary>
        /// Creates new Badge object in database
        /// </summary>
        /// <param name="badge">new Badge</param>
        void CreateBadge(Badge badge);

        /// <summary>
        /// Updates existing badge in database
        /// </summary>
        /// <param name="badge"></param>
        void UpdateBadge(Badge badge);

        /// <summary>
        /// Deletes badge specified by id
        /// </summary>
        /// <param name="badgeId"></param>
        void DeleteBadge(Guid badgeId);

        /// <summary>
        /// Get specific badge by unique id
        /// </summary>
        /// <param name="badgeId"></param>
        /// <returns></returns>
        Badge GetBadge(Guid badgeId);

        /// <summary>
        /// Lists filtered badges
        /// </summary>
        /// <param name="filters">Filters badges</param>
        /// <param name="pageAndOrder"></param>
        /// <returns></returns>s
        List<Badge> ListBadges(IEnumerable<IFilter<BadgeModel>> filters, IPageAndOrderable<BadgeModel> pageAndOrder);

        /// <summary>
        /// Lists all not achieved badges for given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<Badge> ListNotAchievedBadges(Guid accountId);

        /// <summary>
        /// Check all accounts if they dont deserve some badges
        /// </summary>
        void CheckBadgesRequirements();
    }
}
