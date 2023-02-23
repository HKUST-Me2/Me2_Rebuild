// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    public class SexHar_ExampleDialog : ComponentDialog
    {

        public SexHar_ExampleDialog()
            : base(nameof(SexHar_ExampleDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.

        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext turnContext, CancellationToken cancellationToken)
        {
   
            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.Example1().ToAttachment());
            reply.Attachments.Add(Cards.Example2().ToAttachment());
            reply.Attachments.Add(Cards.Example3().ToAttachment());
            reply.Attachments.Add(Cards.Example4().ToAttachment());
            reply.Attachments.Add(Cards.Example5().ToAttachment());
            reply.Attachments.Add(Cards.Example6().ToAttachment());

            // Send the card(s) to the user as an attachment to the activity
            await turnContext.Context.SendActivityAsync(reply, cancellationToken);

            await turnContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);

            return await turnContext.EndDialogAsync();
        }

        
    }
}
