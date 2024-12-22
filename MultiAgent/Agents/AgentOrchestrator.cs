using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace MultiAgent.Agents
{
    public class AgentOrchestrator
    {
        private AgentGroupChat _chat;
        private ChatHistory _chatHistory;
        private List<string> messages = new List<string>();

        public AgentOrchestrator(TravelAgentBuilder travelAgentBuilder, BudgetAgentBuilder budgetAgentBuilder, Kernel kernel)
        {
            var travelAgent = travelAgentBuilder.CreateAgent();
            var budgetAgent = budgetAgentBuilder.CreateAgent();
            _chatHistory = new ChatHistory();


            var terminateFunction = AgentGroupChat.CreatePromptFunctionForStrategy(
              $$$"""
                Determine if the travel suggestion based on the provided budget has been shared to the user. If so, respond with a single word: yes.

                History:

                {{$history}}
                """,
               safeParameterNames: "history"
              );

            var selectionFunction = AgentGroupChat.CreatePromptFunctionForStrategy(
                $$$"""
                Your job is to determine which participant takes the next turn in a conversation according to the action of the most recent participant.
                State only the name of the participant to take the next turn.

                Choose only from these participants:
                - {{{travelAgent.Name}}}
                - {{{budgetAgent.Name}}}

                Always follow these steps when selecting the next participant:
                1) After user input, it is {{{travelAgent.Name}}}'s to provide a range of travel options based on the user's preference.
                2) After {{{travelAgent.Name}}} replies, it's {{{budgetAgent.Name}}}'s turn to find the right travel plan based on the user's budget.
                
                History:
                {{$history}}
                """,
                safeParameterNames: "history"
            );

            _chat = new(travelAgent, budgetAgent)
            {
                ExecutionSettings = new()
                {
                    TerminationStrategy = new KernelFunctionTerminationStrategy(terminateFunction, kernel)
                    {
                        Agents = [budgetAgent],
                        ResultParser = (result) => result.GetValue<string>()?.Contains("yes", StringComparison.OrdinalIgnoreCase) ?? false,
                        HistoryVariableName = "history",
                        MaximumIterations = 10
                    },
                    SelectionStrategy = new KernelFunctionSelectionStrategy(selectionFunction, kernel)
                    {
                        InitialAgent = travelAgent,
                        AgentsVariableName = "agents",
                        HistoryVariableName = "history"
                    }
                }
            };
        }

        public async Task<string> InvokeAgentAsync(string input)
        {
            await _chat.ResetAsync();
            _chat.IsComplete = false;

            foreach (ChatMessageContent chatMessage in _chatHistory)
            {
                _chat.AddChatMessage(chatMessage);
            }

            ChatMessageContent message = new(AuthorRole.User, input);
            _chat.AddChatMessage(message);
            _chatHistory.Add(message);

            StringBuilder sb = new();
            await foreach (ChatMessageContent response in this._chat.InvokeAsync())
            {
                
                Console.WriteLine($"# {response.AuthorName} # {response}");
                messages.Add(response.Content);
            }

            var lastMessage = messages.LastOrDefault();
            _chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, lastMessage));

            return lastMessage;
        }
    }
}
