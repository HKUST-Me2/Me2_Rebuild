// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public class DialogAndWelcomeBot<T> : DialogBot<T> where T : Dialog
    {
        public DialogAndWelcomeBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
            : base(conversationState, userState, dialog, logger)
        {
        }

        // Messages sent to the user.
        private const string WelcomeMessage = "I'm **Me2**. I'm here to help you with any issues related to sexual harassment.";

        private const string InfoMessage = "We care a lot about confidentiality. " +
                                            " **ONLY you** can access the chats unless you choose to connect with the University. " +
                                            "Please also note that the information I provide is for **informational purpose** only. " +
                                            "Consult an attorny for legal advice.";

        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    if (Globals.DEBUG_MODE==1) {await turnContext.SendActivityAsync("#ID=1");}
                    await turnContext.SendActivityAsync($"👋Hi there! {WelcomeMessage}", cancellationToken: cancellationToken);

                    if (Globals.DEBUG_MODE==1) {await turnContext.SendActivityAsync("#ID=2");}
                    await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);

                    await turnContext.SendActivityAsync("⌨️Type anything to start the bot:", cancellationToken: cancellationToken);
                }
            }
        }
    }     
        
}