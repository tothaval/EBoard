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
