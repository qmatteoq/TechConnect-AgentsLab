using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Web;
using TravelAgency.Shared.Models;

namespace SingleAgent.Plugins
{
    public class DestinationsPlugin
    {
        [KernelFunction, Description("Get a list of available destinations")]
        public async Task<List<Destination>> GetDestinations([Description("A list of tags that describes the type of trip the customer is trying to organize. You can only choose among the following values: City, Culture, Art, Fashion, Landmarks, Shopping, Technology, Food, Beaches, Nightlife, Architecture")] string[] tags)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5074/");

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var tag in tags)
            {
                query.Add("tag", tag);
            }

            var queryString = query.ToString();
            var result = await client.GetFromJsonAsync<List<Destination>>($"/destinations?{queryString}");

            return result;
        }
    }
}
