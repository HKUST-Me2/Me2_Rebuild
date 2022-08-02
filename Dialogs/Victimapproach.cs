// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public class SexHar_VictimDialog : ComponentDialog
    {
        public SexHar_VictimDialog()
            : base(nameof(SexHar_VictimDialog))
        {
            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                //WhichStoryAsync,
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.

        private const string text1 = "If you're experiencing sexual harassment, please SAY NO directly. Make it clear to the harasser that you do not welcome the behaviour and it must stop, for example by yelling at him/her to stop, pushing him/her away, or sending a text message to voice your objection to such behaviour.";
        private const string text2 = "You have options—when you are ready. This information is a starting point to help you think through what options may be best for you. While navigating your options below, keep in mind that the information may not be complete and that some options may not be applicable to your circumstance. None of the information provided is legal advice.";
        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext turnContext, CancellationToken cancellationToken)
        {
           
            //_logger.LogInformation("MainDialog.ShowCardStepAsync");
            if (Globals.DEBUG_MODE==1) {await turnContext.Context.SendActivityAsync(MessageFactory.Text("ID:4-2-2-2"), cancellationToken);}
            await turnContext.Context.SendActivityAsync(MessageFactory.Text(text1), cancellationToken);
            
            if (Globals.DEBUG_MODE==1) {await turnContext.Context.SendActivityAsync(MessageFactory.Text("ID:4-2-2-3"), cancellationToken);}
            await turnContext.Context.SendActivityAsync(MessageFactory.Text(text2), cancellationToken);

            
            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            if (Globals.DEBUG_MODE==1) {await turnContext.Context.SendActivityAsync(MessageFactory.Text("ID:4-2-2-4"), cancellationToken);}
            
            var attachments = new List<Attachment>();
            
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            // reply.Attachments.Add(Cards.GetVictimCard1().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard2().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard3().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard4().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard5().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard6().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard7().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard8().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard9().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard10().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard11().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard12().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard13().ToAttachment());
            // reply.Attachments.Add(Cards.GetVictimCard14().ToAttachment());

            
            reply.Attachments.Add(Cards.Victim1());
            reply.Attachments.Add(Cards.Victim2());
            reply.Attachments.Add(Cards.Victim3());
            reply.Attachments.Add(Cards.Victim4());
            reply.Attachments.Add(Cards.Victim5());
            reply.Attachments.Add(Cards.Victim6());
            reply.Attachments.Add(Cards.Victim7());
            reply.Attachments.Add(Cards.Victim8());
            reply.Attachments.Add(Cards.Victim9());
            reply.Attachments.Add(Cards.Victim10());
            reply.Attachments.Add(Cards.Victim11());
            reply.Attachments.Add(Cards.Victim12());
            reply.Attachments.Add(Cards.Victim13());
            reply.Attachments.Add(Cards.Victim14());

            await turnContext.Context.SendActivityAsync(MessageFactory.Text(" **Try srolling the cards if you can, so as to show all the stories** "), cancellationToken);


            //var response = turnContext.Activity.CreateReply();
            //response.Attachments = new List<Attachment>() {Cards.Victim1()};
            // Send the card(s) to the user as an attachment to the activity
            await turnContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another card."), cancellationToken);

            await turnContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);

            return await turnContext.EndDialogAsync();
        }

        
    }
}
