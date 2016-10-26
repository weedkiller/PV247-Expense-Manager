﻿using System.Data.Entity;
using DAL.Entities;

namespace DAL
{
    internal class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext() : base("ExpenseManagerDB") { }

        public ExpenseDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<Badge> Badges { get; set; }
        public DbSet<CostInfo> CostInfos { get; set; }
        public DbSet<CostInfoPaste> CostInfoPastes { get; set; }
        public DbSet<CostType> CostTypes { get; set; }
        public DbSet<Paste> Pastes { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<UserPasteAccess> UserPasteAccesses { get; set; }
    }
}
