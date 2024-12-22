using Microsoft.Agents.Protocols.Adapter;
using Microsoft.Agents.Protocols.Primitives;
using MultiAgent.Agents;

namespace MultiAgent.Bots
{
    public class BasicBot : ActivityHandler
    {
        private AgentOrchestrator _agentOrchestrator;

        public BasicBot(AgentOrchestrator agentOrchestrator)
        {
            _agentOrchestrator = agentOrchestrator;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var typingActivity = turnContext.Activity.CreateReply();
            typingActivity.Type = ActivityTypes.Typing;
            await turnContext.SendActivityAsync(typingActivity, cancellationToken);

            var response = await _agentOrchestrator.InvokeAgentAsync(turnContext.Activity.Text);
            if (response == null)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, I couldn't get a response at the moment."), cancellationToken);
                return;
            }

            var textResponse = MessageFactory.Text(response);

            await turnContext.SendActivityAsync(textResponse, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            // When someone (or something) connects to the bot, a MembersAdded activity is received.
            // For this sample,  we treat this as a welcome event, and send a message saying hello.
            // For more details around the membership lifecycle, please see the lifecycle documentation.
            IActivity message = MessageFactory.Text("Hello and Welcome! I'm here to help with all your travel needs!");

            // Send the response message back to the user. 
            await turnContext.SendActivityAsync(message, cancellationToken);
        }
    }
}
