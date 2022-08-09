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
    public class RecordCaseDialog : ComponentDialog
    {

        public RecordCaseDialog()
            : base(nameof(RecordCaseDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ChoicePrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                RecordCaseGetChoice,
                RecordCaseExplain,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> RecordCaseGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"I\'m going to ask you some questions about what happened. You can edit your responses later if you like.  And no one will see what we discuss unless you decide to submit a report after we finish chatting."), cancellationToken);

            await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"This record is optional and for informational purposes only. Nothing in the form is legal advice."), cancellationToken);

             var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(Globals.prompt_text),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = RecordCaseGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RecordCaseExplain(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Yes, I understand and I'm ready.":
                {
                    await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"Before I ask you to describe what happened, we’ll go through (1) when this happened, (2) where it happened, and (3) who was involved."), cancellationToken);

                    await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"You can always choose \"Skip\" or \"I\'d rather not say\" in response to specific questions.You can edit your responses later if you like"), cancellationToken);
                    
                    var promptOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text(Globals.prompt_text),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = RecordCaseConfirmChoice(),
                        Style = ListStyle.HeroCard,
                    };

                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
                }
                    
                
                case "I want to know more about how this works.":
                {
                    await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"Purpose: There are many reasons you might choose to record the case through me. You might want to record what happened as a way to process it, or because it may feel less difficult to share a written document than recount your story verbally. You might also choose to record because you want to help protect yourself from any possible allegations of false accusations. What you do with this record is up to you. You can keep it for yourself, show this record to the University, or the police, or provide it to your therapist, an attorney, or another provider."), cancellationToken);

                    await stepContext.Context.SendActivityAsync(
                            MessageFactory.Text($"Who can see this and what to write: Without your permission, no one, including the university and whoever manages me, will know your identity. If you are uncomfortable answering any of my questions, skip it. Leave as many questions as you\'d like blank. I can just be a guide for what you might be asked in the future if you choose to report."), cancellationToken);


                    var promptOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text(Globals.prompt_text),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = RecordCaseConfirmChoice(),
                        Style = ListStyle.HeroCard,
                    };

                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
                }
            
                default:
                    return await stepContext.EndDialogAsync(); 
            }         
        }

        private IList<Choice> RecordCaseGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Yes, I understand and I'm ready."},
                new Choice() { Value = "I want to know more about how this works."},
            };

            return cardOptions;
        }

        private IList<Choice> RecordCaseConfirmChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Yes, I understand and I'm ready."},
            };

            return cardOptions;
        }

    }
}
