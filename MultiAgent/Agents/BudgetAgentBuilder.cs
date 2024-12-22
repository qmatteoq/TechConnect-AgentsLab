using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MultiAgent.Plugins;

namespace MultiAgent.Agents
{
    public class BudgetAgentBuilder
    {
        private readonly Kernel _kernel;

        private const string AgentName = "BudgetAgent";
        private const string AgentInstructions = """
            You are a friendly assistant that helps people finding the right plan for a trip based on their budget.
            The user will share with you a possible list location they want to visit and the budget they have.
            You have access to a tool that gives you a list of locations and different trip plans with different budgets.
            You must suggest to user only destinations that are within their budget.
            """;

        public BudgetAgentBuilder(Kernel kernel)
        {
            _kernel = kernel;
        }

        public ChatCompletionAgent CreateAgent()
        {
            ChatCompletionAgent agent =
                new()
                {
                    Instructions = AgentInstructions,
                    Name = AgentName,
                    Kernel = _kernel,
                    Arguments = new KernelArguments(new OpenAIPromptExecutionSettings()
                    {
                        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),

                    }),
                };

            // Give the agent some tools to work with
            agent.Kernel.ImportPluginFromType<BudgetPlugin>();

            return agent;
        }
    }
}
