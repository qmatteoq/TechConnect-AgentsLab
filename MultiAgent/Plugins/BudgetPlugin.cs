using Microsoft.SemanticKernel;
using MultiAgent.Models;
using System.ComponentModel;

namespace MultiAgent.Plugins
{
    public class BudgetPlugin
    {
        List<BudgetPlan> plans = new List<BudgetPlan>();

        public BudgetPlugin()
        {
            plans = new List<BudgetPlan>
            {
                new BudgetPlan
                {
                    Location = "Paris",
                    Country = "France",
                    Description = "City of Love",
                    MaximumBudget = 1000
                },
                new BudgetPlan
                {
                    Location = "London",
                    Country = "England",
                    Description = "City of Rain",
                    MaximumBudget = 2000
                },
                new BudgetPlan
                {
                    Location = "New York",
                    Country = "USA",
                    Description = "City of Skyscrapers",
                    MaximumBudget = 3000
                },
                new BudgetPlan
                {
                    Location = "Tokyo",
                    Country = "Japan",
                    Description = "City of Lights",
                    MaximumBudget = 4000
                }
            };
        }

        [KernelFunction, Description("Get the list of budget plans based on the maximum budget")]

        public List<BudgetPlan> GetBudgetPlans([Description("The maximum budget")]double maximumBudget)
        {
            return plans.Where(p => p.MaximumBudget <= maximumBudget).ToList();
        }
    }
}
