using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EEP_BudgetWatcher.Models
{
    [Serializable]
    public class BudgetOverviewModel
    {
        public BudgetOverviewModel()
        {
            Budgets = [];            
        }

        public BudgetOverviewModel(List<Budget> budgets)
        {
            Budgets = budgets;
        }

        public List<Budget> Budgets { get; set; }
    }
}
