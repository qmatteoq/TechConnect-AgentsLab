using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MultiAgent.Plugins;

namespace MultiAgent.Agents
{
    public class TravelAgentBuilder
    {
        private readonly Kernel _kernel;

        private const string AgentName = "TravelAgent";
        private const string AgentInstructions = """
            You are a friendly assistant that helps people planning a trip.
            Your goal is to provide suggestions for a place to go based on the trip description of the user. 
            You have access to a tool that gives you a list of available places you can suggest. 
            You can suggest only a place which is incuded in this list.
            """;

        public TravelAgentBuilder(Kernel kernel)
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
            agent.Kernel.ImportPluginFromType<DestinationsPlugin>();

            return agent;
        }
    }
}
