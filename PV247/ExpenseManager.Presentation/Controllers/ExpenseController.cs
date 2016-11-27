using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseManager.Business.DataTransferObjects;
using ExpenseManager.Business.DataTransferObjects.Enums;
using ExpenseManager.Business.DataTransferObjects.Filters;
using ExpenseManager.Business.DataTransferObjects.Filters.CostInfos;
using ExpenseManager.Business.Facades;
using ExpenseManager.Presentation.Authentication;
using ExpenseManager.Presentation.Models.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing expenses
    /// </summary>
    [Authorize]
    [Authorize(Policy = "HasAccount")]
    public class ExpenseController : BaseController
    {
        private readonly BalanceFacade _balanceFacade;

        private const int NumberOfExpensesPerPage = 10;

        /// <summary>
        /// Constructor for ExpenseController
        /// </summary>
        /// <param name="balanceFacade"></param>
        /// <param name="mapper"></param>
        /// <param name="currentAccountProvider"></param>
        public ExpenseController(BalanceFacade balanceFacade, Mapper mapper, ICurrentAccountProvider currentAccountProvider) : base(currentAccountProvider, mapper)
        {
            _balanceFacade = balanceFacade;
        }

        #region Nonpermanent expenses

        /// <summary>
        /// Displays expenses for loged-in user
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(IndexFilterViewModel filterModel)
        {
            var account = CurrentAccountProvider.GetCurrentAccount(HttpContext.User);
            if (filterModel.DateTo != null)
            {
                if (filterModel.MoneyFrom != null)
                {
                    if (filterModel.MoneyTo != null)
                    {
                        if (filterModel.CostTypeId != null)
                        {
                            var filters = new List<IFilter<CostInfo>>
                            {
                                new CostInfosByAccountId(account.Id),
                                new CostInfosByItsPeriodicity(Periodicity.None),
                                new CostInfosByCreatedFrom(filterModel.DateFrom),
                                new CostInfosByCreatedTo(filterModel.DateTo.Value),
                                new CostInfosByMoneyFrom(filterModel.MoneyFrom.Value),
                                new CostInfosByMoneyTo(filterModel.MoneyTo.Value),
                                new CostInfosByTypeId(filterModel.CostTypeId.Value)
                            };
                            PageAndOrderFilter pageAndOrder = new PageAndOrderFilter();

                            pageAndOrder.PageNumber = filterModel.PageNumber ?? 1;
                            pageAndOrder.PageSize = NumberOfExpensesPerPage;

                            filterModel.Expenses = GetFilteredExpenses(filters, pageAndOrder);
                            filterModel.PageCount = (int)Math.Ceiling(_balanceFacade.GetCostInfosCount(filters, null) / (double)NumberOfExpensesPerPage);
                        }
                    }
                }
            }
            filterModel.CostTypes = GetAllCostTypes();
            filterModel.CurrentUser = Mapper.Map<Models.User.IndexViewModel>(CurrentAccountProvider.GetCurrentUser(HttpContext.User));
            return View(filterModel);
        }


        /// <summary>
        /// Displays form for creating new expense
        /// </summary>
        [Authorize(Policy = "HasFullRights")]
        public IActionResult Create([FromQuery] string errorMessage = null)
        {
            var model = new CreateViewModel
            {
                CostTypes = GetAllCostTypes(),
                ErrorMessage = errorMessage
            };
            return View(model);
        }


        /// <summary>
        /// Stores new expense
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HasFullRights")]
        public IActionResult Store(CreateViewModel costInfoViewModel)
        {
            var costType = _balanceFacade.GetItemType(costInfoViewModel.TypeId);

            if (!ModelState.IsValid || costType == null)
            {
                return RedirectToAction("Create", new { errorMessage = ExpenseManagerResource.InvalidInputData });
            }

            var costInfo = Mapper.Map<CostInfo>(costInfoViewModel);

            var account = CurrentAccountProvider.GetCurrentAccount(HttpContext.User);

            costInfo.AccountId = account.Id;
            costInfo.Created = DateTime.Now;
            costInfo.Periodicity = Periodicity.None;

            _balanceFacade.CreateItem(costInfo);

            return RedirectToAction("Index", new { successMessage = ExpenseManagerResource.ExpenseCreated });
        }

        private List<IndexPermanentExpenseViewModel> GetAllPermanentExpenses(Account account)
        {
            var filters = new List<IFilter<CostInfo>>
            {
                new CostInfosByAccountId(account.Id),
                new CostInfosByItsPeriodicity(Periodicity.Day)
            };

            var expenses = _balanceFacade.ListItems(filters, null);

            filters.Clear();
            filters = new List<IFilter<CostInfo>>
            {
                new CostInfosByAccountId(account.Id),
                new CostInfosByItsPeriodicity(Periodicity.Week)
            };
            expenses.AddRange(_balanceFacade.ListItems(filters, null));

            filters.Clear();
            filters = new List<IFilter<CostInfo>>
            {
                new CostInfosByAccountId(account.Id),
                new CostInfosByItsPeriodicity(Periodicity.Month)
            };
            expenses.AddRange(_balanceFacade.ListItems(filters, null));

            return Mapper.Map<List<IndexPermanentExpenseViewModel>>(expenses);
        }

        #endregion

        #region Permanent expenses

        /// <summary>
        /// Displays permanent expenses
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "HasAccount")]
        public IActionResult PermanentExpensesIndex()
        {
            var account = CurrentAccountProvider.GetCurrentAccount(HttpContext.User);
            var currentUserModel = Mapper.Map<Models.User.IndexViewModel>(CurrentAccountProvider.GetCurrentUser(HttpContext.User));

            var model = new PermanentExpensesIndexViewModel()
            {
                Expenses = GetAllPermanentExpenses(account),
                CurrentUser = currentUserModel
            };

            return View(model);
        }

        /// <summary>
        /// Displays form for creating permanent expenses
        /// </summary>
        [Authorize(Policy = "HasFullRights")]
        public IActionResult CreatePermanentExpense([FromQuery] string errorMessage = null)
        {
            var model = new CreatePermanentExpenseViewModel
            {
                CostTypes = GetAllCostTypes(),
                ErrorMessage = errorMessage
            };
            return View(model);
        }

        /// <summary>
        /// Stores permanent expense
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HasFullRights")]
        public IActionResult StorePermanentExpense(CreatePermanentExpenseViewModel costInfoViewModel)
        {
            var costType = _balanceFacade.GetItemType(costInfoViewModel.TypeId);

            if (!ModelState.IsValid || costType == null)
            {
                return RedirectToAction("CreatePermanentExpense", new { errorMessage = ExpenseManagerResource.InvalidInputData });
            }

            var costInfo = Mapper.Map<CostInfo>(costInfoViewModel);

            var account = CurrentAccountProvider.GetCurrentAccount(HttpContext.User);

            costInfo.AccountId = account.Id;

            _balanceFacade.CreateItem(costInfo);

            return RedirectToAction("Index", "AccountSettings", new { successMessage = ExpenseManagerResource.ExpenseCreated });
        }

        #endregion

        /// <summary>
        /// Deletes expense with given id
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HasFullRights")]
        public IActionResult Delete(
            [FromForm] Guid id,
            [FromForm] string returnRedirect)
        {
            var costInfo = _balanceFacade.GetItem(id);
            var account = CurrentAccountProvider.GetCurrentAccount(HttpContext.User);

            if (costInfo == null || costInfo.AccountId != account.Id)
            {
                return RedirectWithError(ExpenseManagerResource.ExpenseNotDeleted);
            }

            _balanceFacade.DeleteItem(id);

            return Redirect(returnRedirect);
        }

        #region Helpers
        private List<IndexViewModel> GetFilteredExpenses(List<IFilter<CostInfo>> filters, PageAndOrderFilter pageAndOrder)
        {
            var expenses = _balanceFacade.ListItems(filters, pageAndOrder);
            return Mapper.Map<List<IndexViewModel>>(expenses);
        }

        private List<Models.CostType.IndexViewModel> GetAllCostTypes()
        {
            var costTypes = _balanceFacade.ListItemTypes(null, null);
            var costTypeViewModels = Mapper.Map<List<Models.CostType.IndexViewModel>>(costTypes);
            return costTypeViewModels;
        }
        #endregion
    }
}