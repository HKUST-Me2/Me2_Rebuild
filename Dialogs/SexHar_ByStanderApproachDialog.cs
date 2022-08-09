// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    public class ByStanderApproachDialog : ComponentDialog
    {

        public ByStanderApproachDialog()
            : base(nameof(ByStanderApproachDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ByStanderChooseAction,
                ByStanderProvideSuggestions,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        /// <summary>
        /// Requesting users to choose option in a hero card
        /// (i.e. seeing someone need help OR someone ask for help)
        /// </summary>
        /// <param name="stepContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<DialogTurnResult> ByStanderChooseAction(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2-4-1/#ID:4-2-2-4-2"), cancellationToken);}

            // A List of options, which can be chosen by user.
            var ByStanderChooseCardOptions = new List<Choice>()
            {
                new Choice() { Value = "Seeing someone being sexually harassed", Synonyms = new List<string>() { "Seeing" } },
                new Choice() { Value = "Someone asks you for help", Synonyms = new List<string>() { "Ask" } },
            };

            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("👀Are you seeing someone who is being sexually harassed" +
                                            " or is there someone who asks you for help ?"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
                Choices = ByStanderChooseCardOptions,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);

        }

        private async Task<DialogTurnResult> ByStanderProvideSuggestions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text((((FoundChoice)stepContext.Result).Value)), cancellationToken);

            if (((FoundChoice)stepContext.Result).Value == "Seeing someone being sexually harassed")
            {
                if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID:4-2-2-4-2-2"), cancellationToken);}

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Confront the harasser if circumstances allow," +
                        " and point out what he/she is doing is inappropriate and may amount to sexual harassment."), cancellationToken);
            }
            else  // It is "Ask"
            {
                // Creating a carousel if someone asks user for help.
                var ByStanderAskAttachment = new List<Attachment>();
                // Reply to the activity we received with an activity.
                var ByStanderAskReply = MessageFactory.Attachment(ByStanderAskAttachment);
                ByStanderAskReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID:ID:4-2-2-4-2-2"), cancellationToken);}
                
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk1Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk2Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk3Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk4Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk5Card().ToAttachment());
                // Send the card(s) to the user as an attachment to the activity
                await stepContext.Context.SendActivityAsync(ByStanderAskReply, cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }


        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Adaptive Card", Synonyms = new List<string>() { "adaptive" } },
                new Choice() { Value = "Animation Card", Synonyms = new List<string>() { "animation" } },
                new Choice() { Value = "Audio Card", Synonyms = new List<string>() { "audio" } },
                new Choice() { Value = "Hero Card", Synonyms = new List<string>() { "hero" } },
                new Choice() { Value = "OAuth Card", Synonyms = new List<string>() { "oauth" } },
                new Choice() { Value = "Receipt Card", Synonyms = new List<string>() { "receipt" } },
                new Choice() { Value = "Signin Card", Synonyms = new List<string>() { "signin" } },
                new Choice() { Value = "Thumbnail Card", Synonyms = new List<string>() { "thumbnail", "thumb" } },
                new Choice() { Value = "Video Card", Synonyms = new List<string>() { "video" } },
                new Choice() { Value = "ByStander", Synonyms = new List<string>() { "ByStander" } },
                new Choice() { Value = "All cards", Synonyms = new List<string>() { "all" } },
            };

            return cardOptions;
        }
    }
}
