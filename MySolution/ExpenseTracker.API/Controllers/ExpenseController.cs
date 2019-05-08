using ExpenseTracker.Repository;
using ExpenseTracker.Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseTracker.API.Controllers
{
    [RoutePrefix("api")]
    public class ExpenseController : ApiController
    {
        IExpenseTrackerRepository _repository;
        ExpenseGroupFactory _expenseGroupFactory = new ExpenseGroupFactory();
        ExpenseFactory _expenseFactory = new ExpenseFactory();

        public ExpenseController()
        {
            _repository = new ExpenseTrackerEFRepository(new
                Repository.Entities.ExpenseTrackerContext());
        }

        [Route("expensegroups/{expenseGroupId}/expenses")]
        public IHttpActionResult Get(int expenseGroupId)
        {
            try
            {
                var expenses = _repository.GetExpenses(expenseGroupId);

                if (expenses == null)
                {
                    return NotFound();
                }

                var expenseResult = expenses
                    .ToList()
                    .Select(exp => _expenseFactory.CreateExpense(exp));

                return Ok(expenseResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expensegroups/{expenseGroupId}/expenses/{id}")]
        [Route("expenses/{id}")]
        public IHttpActionResult Get(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;

                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);
                    
                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }
                }

                if (expense != null)
                {
                    var returnValue = _expenseFactory.CreateExpense(expense);
                    return Ok(returnValue);
                }
                else
                {
                    return NotFound();
                }
               
            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }
    }
}
