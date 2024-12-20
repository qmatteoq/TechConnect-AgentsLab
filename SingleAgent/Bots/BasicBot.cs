﻿using Microsoft.Agents.Protocols.Adapter;
using Microsoft.Agents.Protocols.Primitives;

namespace SingleAgent.Bots
{
    public class BasicBot: ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Create a new Activity from the message the user provided and modify the text to echo back.
            IActivity message = MessageFactory.Text($"Echo: {turnContext.Activity.Text}");

            // Send the response message back to the user. 
            await turnContext.SendActivityAsync(message, cancellationToken);
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
