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
    public class data
        {
            public string Year;
            public string Season;
            public string Date;
            public string Time;
            public string DateAdditional;
            public string Place;
            public string PlaceAdditional;
            public string Eyewitness;
            public string Interact;
            public string EyewitnessAdditional;
            public string ToldOthers;
            public string ToldOthersAdditional;
            public string Offender;

        }

        
    public class InputDialog : ComponentDialog
    {
        data MyData = new data();

        public InputDialog()
            : base(nameof(InputDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                YearGetChoice,
                SeasonGetChoice,
                DateGetChoice,
                DateOther,
                TimeGetChoice,
                TimeOther,
                DateAdditional,
                PlaceGetChoice,
                PlaceOther,
                PlaceAdditional,
                EyewitnessGetChoice,
                InteractGetChoice,
                EyewitnessAdditional,
                ToldOthersGetChoice,
                ToldOthersAdditional,
                OffenderGetChoice,
                saveOffenderChoice

            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> YearGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Now let's start with the time. What year did this happen?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = YearGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SeasonGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Year = ((FoundChoice)stepContext.Result).Value;

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"What season was it?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = SeasonGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> DateGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Season = ((FoundChoice)stepContext.Result).Value;

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Do you know the exact date?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = DateGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> DateOther(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
             MyData.Date = ((FoundChoice)stepContext.Result).Value;
            if(MyData.Date == "Other: You can directly type the date in the chat"){
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("type here") }, cancellationToken);
            }
            else return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> TimeGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if(MyData.Date == "Other: You can directly type the date in the chat")
                MyData.Date = ((string)stepContext.Result);
            
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"What time of day was it?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = TimeGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> TimeOther(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
             MyData.Time = ((FoundChoice)stepContext.Result).Value;
            if(MyData.Time == "Other: You can directly type the exact time in the chat"){
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("type here") }, cancellationToken);
            }
            else return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> DateAdditional(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
             if(MyData.Time == "Other: You can directly type the exact time in the chat")
                MyData.Time = ((string)stepContext.Result);
        
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is there anything else you remember about the date? \n Examples: holidays, day of the week, games, school or social events around that time, etc.") }, cancellationToken);

        }

        private async Task<DialogTurnResult> PlaceGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Date = ((string)stepContext.Result);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Where did it happen?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = PlaceGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> PlaceOther(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Place = ((FoundChoice)stepContext.Result).Value;

            if(MyData.Place == "Other: You can directly type in the chat"){
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("type here") }, cancellationToken);
            }
            else return await stepContext.NextAsync();
        }


        private async Task<DialogTurnResult> PlaceAdditional(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
             if(MyData.Place == "Other: You can directly type in the chat")
                MyData.Place = ((string)stepContext.Result);
        

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Are there any other details you remember about the location? \n Examples: exact address, area of campus, intersection, building number, neighborhood, buildings or trees nearby, colors you remember, etc.") }, cancellationToken);

        }

        private async Task<DialogTurnResult> EyewitnessGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.PlaceAdditional = ((string)stepContext.Result);

            var promptOptions = new PromptOptions
            {
                Prompt =  MessageFactory.Text($"Did anyone else see or hear either all or any part of what happened?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = EyewhitnessGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> InteractGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Eyewitness = ((FoundChoice)stepContext.Result).Value;

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Did you interact with anyone right before or after the incident? \n This could be people who were at the scene of the incident or nearby, who saw you or the offender leave, or who helped you call for help."),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = EyewhitnessGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> EyewitnessAdditional(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Interact = ((FoundChoice)stepContext.Result).Value;
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What information do you know about any of the people described above?  \n Examples: how many people, what they saw or heard, their relationship to you, where they may have been standing or sitting, how physically close to you they were.") }, cancellationToken);
        }


        private async Task<DialogTurnResult> ToldOthersGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.EyewitnessAdditional = ((string)stepContext.Result);

            var promptOptions = new PromptOptions
            {
                Prompt =  MessageFactory.Text($"Did you tell anyone about the incident"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = ToldOthersGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ToldOthersAdditional(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.ToldOthers = ((FoundChoice)stepContext.Result).Value;
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What information do you know about the people you told? \n Who you told, what you told them, when you told them, how you told them (on the phone, in person, over text, etc), their relationship to you or the offender, etc") }, cancellationToken);

        }

        private async Task<DialogTurnResult> OffenderGetChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.ToldOthersAdditional = MyData.Date = ((string)stepContext.Result);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Do you know if the offender told anyone about the incident?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = ToldOthersGetChoice(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> saveOffenderChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.Offender = ((FoundChoice)stepContext.Result).Value;

            return await stepContext.EndDialogAsync(); 
        }

        private IList<Choice> YearGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "This year"},
                new Choice() { Value = "Last year"},
                new Choice() { Value = "A different year"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"}
            };

            return cardOptions;
        }

        private IList<Choice> SeasonGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Spring"},
                new Choice() { Value = "Summer"},
                new Choice() { Value = "Fall"},
                new Choice() { Value = "Winter"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

        private IList<Choice> DateGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Other: You can directly type the date in the chat"},
                new Choice() { Value = "I’m not sure"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

        private IList<Choice> TimeGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Morning"},
                new Choice() { Value = "Afternoon"},
                new Choice() { Value = "Evening"},
                new Choice() { Value = "Late Night (after 10 pm)"},
                new Choice() { Value = "Others: You can directly type the exact time in the chat"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

        private IList<Choice> PlaceGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "On Campus"},
                new Choice() { Value = "Off Campus, locally (local bar, local housing complex, etc)"},
                new Choice() { Value = "I’m not sure"},
                new Choice() { Value = "Other: You can directly type in the chat"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

        private IList<Choice> EyewhitnessGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Yes"},
                new Choice() { Value = "No"},
                new Choice() { Value = "I’m not sure"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

        private IList<Choice> ToldOthersGetChoice()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Yes"},
                new Choice() { Value = "No"},
                new Choice() { Value = "I'd rather not say"},
                new Choice() { Value = "Skip"},
            };

            return cardOptions;
        }

    }
}
