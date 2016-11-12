﻿using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Business.Facades;
using ExpenseManager.Business.Infrastructure;
using ExpenseManager.Business.Infrastructure.Mapping.Profiles;
using ExpenseManager.Business.Services.Implementations;
using ExpenseManager.Business.Services.Interfaces;
using ExpenseManager.Database.DataAccess.Queries;
using ExpenseManager.Database.DataAccess.Repositories;
using ExpenseManager.Database.Entities;
using ExpenseManager.Database.Filters;
using ExpenseManager.Database.Infrastructure.ConnectionConfiguration;
using ExpenseManager.Database.Infrastructure.Query;
using ExpenseManager.Database.Infrastructure.Repository;
using ExpenseManager.Database.Infrastructure.UnitOfWork;
using ExpenseManager.Identity;
using ExpenseManager.Presentation.Authentication;
using ExpenseManager.Presentation.Infrastructure.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Riganti.Utils.Infrastructure.Core;

namespace ExpenseManager.Presentation
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {  
            services.Configure<ConnectionOptions>(options => options.ConnectionString = Configuration.GetConnectionString("DefaultConnection"));

            // Configure Identity persistence         
            IdentityDALInstaller.Install(services, Configuration.GetConnectionString("IdentityConnection"));

            // Configure BL
            RegisterBusinessLayerDependencies(services);

            // Configure PL

            services.AddSession();
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseIdentity();

            var x = Configuration.GetSection("FacebookAuthentication")["ClientId"];

            app.UseFacebookAuthentication(new FacebookOptions
            {
                ClientId = Configuration.GetSection("FacebookAuthentication")["ClientId"],
                ClientSecret = Configuration.GetSection("FacebookAuthentication")["ClientSecret"]
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Expense}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Performs BL DI configuration
        /// </summary>
        /// <param name="services">Collection of the service descriptions</param>
        private static void RegisterBusinessLayerDependencies(IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWorkRegistry>(new HttpContextUnitOfWorkRegistry(new ThreadLocalUnitOfWorkRegistry()));

            services.AddSingleton<IUnitOfWorkProvider, ExpenseManagerUnitOfWorkProvider>();

            services.AddTransient(typeof(IRepository<,>),typeof(ExpenseManagerRepository<,>));

            services.AddSingleton(typeof(Mapper), 
                provider => {
                    var config = new MapperConfiguration(cfg => 
                    {
                        cfg.AddProfile<DatabaseToBusinessStandardMapping>();
                        cfg.AddProfile<BussinessToViewModelMapping>();
                    });
                    return config.CreateMapper();
            });

            // Register all repositories
            services.AddTransient<ExpenseManagerRepository<BadgeModel, int>, BadgeRepository>();
            services.AddTransient<BadgeRepository>();
            services.AddTransient<ExpenseManagerRepository<CostInfoModel, int>, CostInfoRepository>();
            services.AddTransient<CostInfoRepository>();
            services.AddTransient<ExpenseManagerRepository<CostTypeModel, int>, CostTypeRepository>();
            services.AddTransient<CostTypeRepository>();
            services.AddTransient<ExpenseManagerRepository<PlanModel, int>, PlanRepository>();
            services.AddTransient<PlanRepository>();
            services.AddTransient<ExpenseManagerRepository<UserModel, int>, UserRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<ExpenseManagerRepository<AccountBadgeModel, int>, AccountBadgeRepository>();
            services.AddTransient<AccountBadgeRepository>();
            services.AddTransient<ExpenseManagerRepository<AccountModel, int>, AccountRepository>();
            services.AddTransient<AccountRepository>();

            // Register all query objects
            services.AddTransient<ExpenseManagerQuery<AccountBadgeModel, AccountBadgeModelFilter>, ListAccountBadgesQuery>();
            services.AddTransient<ListAccountBadgesQuery>();
            services.AddTransient<ExpenseManagerQuery<AccountModel, AccountModelFilter>, ListAccountsQuery>();
            services.AddTransient<ListAccountsQuery>();
            services.AddTransient<ExpenseManagerQuery<BadgeModel, BadgeModelFilter>, ListBadgesQuery>();
            services.AddTransient<ListBadgesQuery>();
            services.AddTransient<ExpenseManagerQuery<CostInfoModel, CostInfoModelFilter>, ListCostInfosQuery>();
            services.AddTransient<ListCostInfosQuery>();
            services.AddTransient<ExpenseManagerQuery<CostTypeModel, CostTypeModelFilter>, ListCostTypesQuery>();
            services.AddTransient<ListCostTypesQuery>();
            services.AddTransient<ExpenseManagerQuery<PlanModel, PlanModelFilter>, ListPlansQuery>();
            services.AddTransient<ListPlansQuery>();
            services.AddTransient<ExpenseManagerQuery<UserModel, UserModelFilter>, ListUsersQuery>();
            services.AddTransient<ListUsersQuery>();
            //TODO add more query objects

            // Register all services
            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<AccountBadgeModel, int, AccountBadge, AccountBadgeModelFilter>), typeof(AccountBadgeService));
            services.AddTransient<IAccountBadgeService, AccountBadgeService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<AccountModel, int, Account, AccountModelFilter>), typeof(AccountService));
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<BadgeModel, int, Badge, BadgeModelFilter>), typeof(BadgeService));
            services.AddTransient<IBadgeService, BadgeService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<CostInfoModel, int, CostInfo, CostInfoModelFilter>), typeof(CostInfoService));
            services.AddTransient<ICostInfoService, CostInfoService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<CostTypeModel, int, CostType, CostTypeModelFilter>), typeof(CostTypeService));
            services.AddTransient<ICostTypeService, CostTypeService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<PlanModel, int, Plan, PlanModelFilter>), typeof(PlanService));
            services.AddTransient<IPlanService, PlanService>();

            services.AddTransient(typeof(ExpenseManagerQueryAndCrudServiceBase<UserModel, int, User, UserModelFilter>), typeof(UserService));
            services.AddTransient<IUserService, UserService>();
            
            services.AddTransient<IBadgeManagerService, BadgeManagerService>();
            //TODO add more services

            // Register all facades
            services.AddTransient<AccountFacade>();
            services.AddTransient<BalanceFacade>();
            //TODO add more facades

            // Presentation layer
            services.AddTransient<ICurrentAccountProvider, CurrentAccountProvider>();

        }
    }
}
