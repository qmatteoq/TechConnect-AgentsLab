using Microsoft.AspNetCore.Mvc;
using TravelAgency.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/destinations", ([FromQuery] string[] tag) =>
{
    List<Destination> destinations = new List<Destination>  
    {
        new Destination
        {
            Location = "Paris",
            Country = "France",
            Description = "Paris is the capital city of France. It is a major European city and a global center for art, fashion, gastronomy, and culture.",
            Tags = new string[] { "City", "Culture", "Art", "Fashion" }
        },
        new Destination
        {
            Location = "New York",
            Country = "United States",
            Description = "New York City is the largest city in the United States. It is known for its iconic landmarks, such as the Statue of Liberty, Times Square, and Central Park.",
            Tags = new string[] { "City", "Culture", "Landmarks", "Shopping" }
        },
        new Destination
        {
            Location = "Tokyo",
            Country = "Japan",
            Description = "Tokyo is the capital city of Japan. It is a vibrant metropolis known for its high-tech innovations, historic temples, and bustling street markets.",
            Tags = new string[] { "City", "Technology", "Culture", "Food" }
        },
        new Destination
        {
            Location = "Sydney",
            Country = "Australia",
            Description = "Sydney is the largest city in Australia. It is famous for its iconic Sydney Opera House, beautiful beaches, and vibrant nightlife.",
            Tags = new string[] { "City", "Beaches", "Culture", "Nightlife" }
        },
        new Destination
        {
            Location = "Rio de Janeiro",
            Country = "Brazil",
            Description = "Rio de Janeiro is a coastal city in Brazil known for its stunning beaches, vibrant carnival celebrations, and iconic Christ the Redeemer statue.",
            Tags = new string[] { "City", "Beaches", "Culture", "Carnival" }
        },
        new Destination
        {
            Location = "Cape Town",
            Country = "South Africa",
            Description = "Cape Town is a coastal city in South Africa known for its stunning natural beauty, diverse wildlife, and vibrant cultural scene.",
            Tags = new string[] { "City", "Nature", "Wildlife", "Culture" }
        },
        new Destination
        {
            Location = "Barcelona",
            Country = "Spain",
            Description = "Barcelona is a vibrant city in Spain known for its stunning architecture, beautiful beaches, and rich cultural heritage.",
            Tags = new string[] { "City", "Architecture", "Beaches", "Culture" }
        },
        new Destination
        {
            Location = "Dubai",
            Country = "United Arab Emirates",
            Description = "Dubai is a modern city in the United Arab Emirates known for its luxury shopping, futuristic skyscrapers, and vibrant nightlife.",
            Tags = new string[] { "City", "Shopping", "Skyscrapers", "Nightlife" }
        }
    };

    var result = destinations
               .Where(d => tag.Any(t => d.Tags.Any(dt => dt.Equals(t, StringComparison.OrdinalIgnoreCase))))
               .ToList();

    return Results.Ok(result);
})
.WithDescription("Returns a list of available destinations based on the provided tags")
.WithName("GetDestinations")
.Produces<List<Destination>>();

app.MapGet("/budget", ([FromQuery] double maximumBudget) =>
{
    var plans = new List<BudgetPlan>
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
                },
                new BudgetPlan
                {
                    Location = "Sydney",
                    Country = "Australia",
                    Description = "City of Beaches",
                    MaximumBudget = 5000
                },
                new BudgetPlan
                {
                    Location = "Rio de Janeiro",
                    Country = "Brazil",
                    Description = "City of Carnival",
                    MaximumBudget = 6000
                },
            };

    return Results.Ok(plans.Where(p => p.MaximumBudget <= maximumBudget).ToList());
})
.WithDescription("Returns a list of available plans based on the maximum budget")
.WithName("GetBudgetPlans")
.Produces<List<BudgetPlan>>(); ;

app.Run();
