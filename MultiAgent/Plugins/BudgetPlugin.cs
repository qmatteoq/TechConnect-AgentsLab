using Microsoft.SemanticKernel;
using TravelAgency.Shared.Models;
using System.ComponentModel;

namespace MultiAgent.Plugins
{
    public class BudgetPlugin
    {
        [KernelFunction, Description("Get the list of budget plans based on the maximum budget provided")]

        public async Task<List<BudgetPlan>> GetBudgetPlans([Description("The maximum budget")] double maximumBudget)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5074/");

            var result = await client.GetFromJsonAsync<List<BudgetPlan>>($"/budget?maximumBudget={maximumBudget}");

            return result;
        }
    }
}
