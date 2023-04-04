// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;


namespace Microsoft.BotBuilderSamples
{
    public class SexHar_OtherStoryDialog : ComponentDialog
    {
        
        public SexHar_OtherStoryDialog()
            : base(nameof(SexHar_OtherStoryDialog))
        {

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                WhichStoryAsync,
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // 1. Prompts the user if the user is not in the middle of a dialog.
        // 2. Re-prompts the user when an invalid input is received.
        private async Task<DialogTurnResult> WhichStoryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2-2-1/#ID:4-2-2-2-2"), cancellationToken);}
            // Create the PromptOptions which contain the prompt and re-prompt messages.
            // PromptOptions also contains the list of choices available to the user.
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("What kind of stories do you want to know? Note that these stories are all from the research of Equal Opportunity Committee."),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = GetChoices(),
                Style = ListStyle.HeroCard,
            };

            // Prompt the user with the configured PromptOptions.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.
        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Sending an instruction of scrolling cards.
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(" **Try srolling the cards if you can, so as to show all the stories** "), cancellationToken);

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            
            // Decide which type of card(s) we are going to show the user
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Online sexual harassment":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-1"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard1().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard2().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard3().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;
                
                case "Verbal harassment":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-2"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard4().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard5().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard6().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;

                case "Sexual harassment off campus":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-3"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard7().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard8().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;

                case "Inappropraite physical contact":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-4"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard9().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard10().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard11().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;

                case "Orientation games and Student activities":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-5"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard12().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard13().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard14().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;
                
                case "Sexual assult or attenpted rape":
                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-2-2-2-2-6"), cancellationToken);}

                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(Cards.GetHeroCard14().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard15().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard16().ToAttachment());
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;
                
                default:
                    reply.Attachments.Add(Cards.GetHeroCard17().ToAttachment());
                    break;
            }

            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }

        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Verbal harassment"},
                new Choice() { Value = "Inappropraite physical contact"},
                new Choice() { Value = "Orientation games and Student activities"},
                new Choice() { Value = "Sexual assult or attenpted rape"},
                new Choice() { Value = "Sexual harassment off campus"},
                new Choice() { Value = "Online sexual harassment"}    
            };

            return cardOptions;
        }
    }
}
