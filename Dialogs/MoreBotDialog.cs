// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples
{

    public class MoreBotDialog : ComponentDialog
    {
        public MoreBotDialog()
            : base(nameof(MoreBotDialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    Options2Async,
                }));
                InitialDialogId = nameof(WaterfallDialog);

        }
        private async Task<DialogTurnResult> SelectionStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-3-1"), cancellationToken);}

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(""),
                RetryPrompt = MessageFactory.Text("Please choose an option from the list."),
                Choices = GetChoices2(),
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> Options2Async(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // First/ Main branch. This triggers other diaglogs.
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "What does Me2 Chatbot do?":

                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-3-1-1-1"), cancellationToken);}

                    await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("Me2 Chatbot offers two features at this moment:\n1. Education about sexual harassment\n2. Documentation of sexual harassment cases"), cancellationToken);
                    break;
                case "How can Me2 Chatbot help me?":

                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-3-1-2-1"), cancellationToken);}

                    await stepContext.Context.SendActivityAsync(
                                    MessageFactory.Text("If you have experienced sexual harassment, Me2 Chatbot helps you to document the details for future reporting. If you want to know more about sexual harassment, Me2 Chatbot acts as a platform for you to learn more about it."), cancellationToken);
                    break;
                case "Can I trust Me2 Chatbot with my private information?":

                    if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"#ID:4-3-1-3-1"), cancellationToken);}

                    await stepContext.Context.SendActivityAsync(
                                    MessageFactory.Text("Your information is safe with us. Me2 Chatbot has built the system so that the developers cannot see what information you input. Only the school will know your documentation details when you decide to report it."), cancellationToken);
                    break;
                default:
                    break;
            }
            return await stepContext.EndDialogAsync(); 
        }

        private IList<Choice> GetChoices2()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "What does Me2 Chatbot do?"},
                new Choice() { Value = "How can Me2 Chatbot help me?"},
                new Choice() { Value = "Can I trust Me2 Chatbot with my private information?"},
            };

            return cardOptions;
        }
    }
}
