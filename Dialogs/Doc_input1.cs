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

using AdaptiveCards;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.BotBuilderSamples.Utilities;

namespace Microsoft.BotBuilderSamples.Utilities
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
            public string multiplechoice;       // Should be a list (?)
            public string remember;
            public string numOfOffender;
            public string nameOfOffender;
            public string infoOfOffender;
            public string infoOfThesePeople;
            public List<Attachment> attachmentDoc; // Store Attachment 

        }
    

    public class InputDialog : ComponentDialog
    {
        data MyData = new data();
        private int EditQuestion = 999;

        private readonly ToDoLUISRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly string UserValidationDialogID = "UserValidationDlg";

        public InputDialog(ToDoLUISRecognizer luisRecognizer, IConfiguration configuration, CosmosDBClient cosmosDBClient)
            : base(nameof(InputDialog))
        {
            _luisRecognizer = luisRecognizer;
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;
   
            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt)));
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
                saveOffenderChoice,
                DocMultipleChoice,
                HandleResponseAsync,
                Documentation31,
                Documentation32,
                Documentation33,
                Documentation34,
                Documentation36,
                Documentation38,
                Documentation39,
                Documentation41,
                Documentation43,
                Documentation44,
                Documentation46,
                ShowAllAnswer,
                EditingResponse,
                GetEditingResponse,
                WaystoHandleRecord,
                haveIDStepAsync,
                UserExistsStepAsync,
                // saveStepAsync,
                SummaryStepAsync,
                ShowTasksStepAsync,
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

            return await stepContext.NextAsync(); 
        }

        // PART 2
        private async Task<DialogTurnResult> DocMultipleChoice(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            //var adaptiveCardJson = File.ReadAllText("./AdaptiveJsons/toaddres_adaptivecard.json");
            //var adaptiveCardAttachment = new Attachment()
            //{
            //    ContentType = AdaptiveCard.ContentType,
            //    Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\DocumentationMC.json")).Card
            //};
            //var reply = stepContext.Context.Activity.CreateReply();
            //reply.Attachments = new List<Attachment>() { Cards.DOCMC() };
            // await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = reply }, cancellationToken);

            var message = MessageFactory.Text("");
            message.Attachments = new List<Attachment>() { Cards.DOCMC() };
            await stepContext.Context.SendActivityAsync(message, cancellationToken);

            // Display the Adaptive Card
            //var cardPath = Path.Combine(".", "AdaptiveCard.json");
            //var cardJson = File.ReadAllText(cardPath);
            //var cardAttachment = new Attachment()
            //{
            //    ContentType = "application/vnd.microsoft.card.adaptive",
            //    Content = JsonConvert.DeserializeObject(cardJson),
            //};
            //var message = MessageFactory.Text("");
            //message.Attachments = new List<Attachment>() { cardAttachment };
            //await stepContext.Context.SendActivityAsync(message, cancellationToken);

            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> HandleResponseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Debug 1");
            // Do something with step.result
            // Adaptive Card submissions are objects, so you likely need to JObject.Parse(step.result)
            var txt = stepContext.Context.Activity.Text;

            dynamic val = stepContext.Context.Activity.Value;
            await stepContext.Context.SendActivityAsync("Debug 2");
            // Check if the activity came from a submit action
            if (string.IsNullOrEmpty(txt) && val != null)
            {
                await stepContext.Context.SendActivityAsync("Debug 3");
                // Retrieve the data from the id_number field
                var num = val.OtherInfo;
                await stepContext.Context.SendActivityAsync("it is");
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(num));
                var test = val.myColour;
                await stepContext.Context.SendActivityAsync("Debug 4");
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(test));

                // . . .
            }

            await stepContext.Context.SendActivityAsync($"INPUT: {stepContext.Result}");
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> Documentation31(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 31
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Nonconsensual sexual contact is never okay. Just because you didn’t say no, " +
            "doesn’t mean you gave consent.You can consent to one thing without consenting to another thing, and you can withdraw consent at any time."), cancellationToken);
            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> Documentation32(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 32
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Why do you think what happened was not okay? Check any that apply. " +
            "Like all questions, it is okay to leave this question blank, especially if you are still processing what happened. " +
            "Leaving this question blank does not mean that what happened to you was not wrong."), cancellationToken);
            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> Documentation33(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //ID:4-4-1-2-3-1-33
            #region Card DocumentationPart3MC1
            var DocumentationPart3MC1 = new List<Choice>()
            {
                new Choice() { Value = "I was not asked whether I wanted to do it", Synonyms = new List<string>() { "I was not asked whether I wanted to do it" } },
                new Choice() { Value = "I did not say “yes” (or a word that means “yes”) verbally to the sexual interaction at all", Synonyms = new List<string>() { "I did not say “yes” (or a word that means “yes”) verbally to the sexual interaction at all" } },
                new Choice() { Value = "I did not say “yes” (or a word that means “yes”) verbally to the particular type of sexual  contact", Synonyms = new List<string>() { "I did not say “yes” (or a word that means “yes”) verbally to the particular type of sexual  contact" } },
                new Choice() { Value = "I was not able to react when it started happening (because it happened quickly, or I was in shock, or I was confused about what was going on)", Synonyms = new List<string>() { "I was not able to react when it started happening (because it happened quickly, or I was in shock, or I was confused about what was going on)" } },
                new Choice() { Value = "I gave non-verbal cues that I didn’t want it (this could include a distressed look, a push, a shaking of the head, etc.)", Synonyms = new List<string>() { "I gave non-verbal cues that I didn’t want it (this could include a distressed look, a push, a shaking of the head, etc.)" } },
                new Choice() { Value = "I said no (or a word that means “no”) to a part of it", Synonyms = new List<string>() { "I said no (or a word that means “no”) to a part of it" } },
                new Choice() { Value = "I didn’t want it (or at least some part of it)", Synonyms = new List<string>() { "I didn’t want it (or at least some part of it)" } },
                new Choice() { Value = "I was not able to give consent (examples: under 18, asleep, intoxicated)", Synonyms = new List<string>() { "I was not able to give consent (examples: under 18, asleep, intoxicated)" } },
                new Choice() { Value = "I don’t really remember what happened, potentially because I was too intoxicated or experienced head trauma, which is a sign that I couldn’t or didn’t consent", Synonyms = new List<string>() { "I don’t really remember what happened, potentially because I was too intoxicated or experienced head trauma, which is a sign that I couldn’t or didn’t consent" } },
                new Choice() { Value = "I was coerced or pressured (examples: repeatedly pestered over and over/or, asked by someone who is in a position of power over me)", Synonyms = new List<string>() { "I was coerced or pressured (examples: repeatedly pestered over and over/or, asked by someone who is in a position of power over me)" } },
                new Choice() { Value = "I was afraid for my safety", Synonyms = new List<string>() { "I was afraid for my safety" } },
                new Choice() { Value = "I was afraid of other consequences if I didn’t say yes (getting a bad grade, being shunned, being shamed, being taunted, etc.)", Synonyms = new List<string>() { "I was afraid of other consequences if I didn’t say yes (getting a bad grade, being shunned, being shamed, being taunted, etc.)" } },
                new Choice() { Value = "Other: You can directly type in the chat", Synonyms = new List<string>() { "Other: You can directly type in the chat" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("Provide Options: *can choose multiple"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
                Choices = DocumentationPart3MC1,
                Style = ListStyle.HeroCard,
            };
            #endregion
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> Documentation34(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.multiplechoice = ((FoundChoice)stepContext.Result).Value; 
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 34
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "What happened? Include anything you are able to remember around what you felt, " +
                    "saw, heard, smelled, tasted, or anything you can’t forget about your experience or experiences with the offender(s).",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation36(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //await stepContext.Context.SendActivityAsync($"INPUT: {stepContext.Result}");
            MyData.remember = ((string)stepContext.Result); 
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 36
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "How many offenders were there? It’s okay if you are not sure, just put what you can remember.",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation38(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.numOfOffender = ((string)stepContext.Result);
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 38
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "What is their name?/What are their names? (if known)",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation39(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.nameOfOffender = ((string)stepContext.Result); 
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 39
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "Do you know any other information about the offender(s)? Examples: " +
                        "cell phone number, what job they have, if you’ve seen them before, how you know them, any physical characteristics " +
                        "(hair color, identifiable marks, tattoos, clothing, moles or birthmarks), or anything you could not forget about them.",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation41(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.infoOfOffender = ((string)stepContext.Result);
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 41
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "What information do you know about these people?",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation43(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.infoOfThesePeople = ((string)stepContext.Result);
            //ID: 4 - 4 - 1 - 2 - 3 - 1 - 43
            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "If you choose to report what happened to the university, the police, " +
                        "the press, or in a lawsuit, they may ask you for electronic and/or physical evidence of what happened. If you don't have these things, " +
                        "it doesn't mean that what happened is any less real.",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.NextAsync(); //PromptAsync(nameof(TextPrompt), opts);
        }

        private async Task<DialogTurnResult> Documentation44(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.DocLongText());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> Documentation46(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //ID:4-4-1-2-3-1-45
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text("What types of evidence do you have now (or think might exist)? " +
            //    "You can send attachments in this chat."), cancellationToken);
            //return await stepContext.NextAsync(null);
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Type = ActivityTypes.Message,
                    Text = "What types of evidence do you have now (or think might exist)? You can send attachments in this chat.",
                }
            };

            // Display a Text Prompt and wait for input
            return await stepContext.PromptAsync(nameof(AttachmentPrompt), opts);
        }

        private async Task<DialogTurnResult> UploadAttachmentAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MyData.attachmentDoc = (List<Attachment>)stepContext.Result;
            List<Attachment> attachments = (List<Attachment>)stepContext.Result;
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Attachment Gotcha {stepContext.Result}"), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.multiplechoice), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.remember), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.numOfOffender), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.nameOfOffender), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.infoOfOffender), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.infoOfThesePeople), cancellationToken);
            // return await stepContext.EndDialogAsync();
            return await stepContext.NextAsync(null, cancellationToken);
            // https://stackoverflow.com/questions/58336861/accepting-attachments-on-a-waterfall-dialog-and-storing-them-locally-in-bot-fram
        }

        // Modification
        // TODO 
        public bool IsAccess;
        private async Task<DialogTurnResult> ShowAllAnswer(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            IsAccess = false;
            if (IsAccess)
            {
                User.UserID = "HL5PQZX"; // hard code this for testing 
                List<ToDoTask> toDoTasks = await _cosmosDBClient.QueryItemsAsync("HL5PQZX", Configuration["CosmosEndPointURI"], Configuration["CosmosPrimaryKey"], Configuration["CosmosDatabaseId"], Configuration["CosmosContainerID"], Configuration["CosmosPartitionKey"]);
                if (toDoTasks.Count == 0)
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("You don't have any tasks added."), cancellationToken);
                    return await stepContext.EndDialogAsync(null, cancellationToken);
                }
                
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(toDoTasks[0].Year), cancellationToken);

                MyData.Year = toDoTasks[0].Year;
                MyData.Season = toDoTasks[0].Season;
                MyData.Date = toDoTasks[0].Date;
                MyData.Time = toDoTasks[0].Time;
                MyData.DateAdditional = toDoTasks[0].DateAdditional;
                MyData.Place = toDoTasks[0].Place;
                MyData.PlaceAdditional = toDoTasks[0].PlaceAdditional;
                MyData.Eyewitness = toDoTasks[0].Eyewitness;
                MyData.Interact = toDoTasks[0].Interact;
                MyData.EyewitnessAdditional = toDoTasks[0].EyewitnessAdditional;
                MyData.ToldOthers = toDoTasks[0].ToldOthers;
                MyData.ToldOthersAdditional = toDoTasks[0].ToldOthersAdditional;
                MyData.Offender = toDoTasks[0].Offender;
                MyData.multiplechoice = toDoTasks[0].multiplechoice;  
                MyData.remember = toDoTasks[0].remember;
                MyData.numOfOffender = toDoTasks[0].numOfOffender;
                MyData.nameOfOffender = toDoTasks[0].nameOfOffender;
                MyData.infoOfOffender = toDoTasks[0].infoOfOffender;
                MyData.infoOfThesePeople = toDoTasks[0].infoOfThesePeople;
                MyData.attachmentDoc = toDoTasks[0].attachemntDoc; 
            }


            MyData.attachmentDoc = (List<Attachment>)stepContext.Result;
            List<Attachment> attachments = (List<Attachment>)stepContext.Result;

            if (Globals.DEBUG_MODE == 1)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Attachment Gotcha {stepContext.Result}"), cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"ID: 4-4-1-2-3-1-47"), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's your response: \n\n" +
                $"**1. What year did this happen?** \n\n" +
                $"{MyData.Year} \n\n" +
                $"**2. What season was it?** \n\n" +
                $"{MyData.Season} \n\n" +
                $"**3. Do you know the exact date?** \n\n" +
                $"{MyData.Date} \n\n" +
                $"**4. What time of day was it?** \n\n" +
                $"{MyData.Time} \n\n" +
                $"**5. Is there anything else you remember about the date?** \n\n" +
                $"{MyData.DateAdditional} \n\n" +
                $"**6. Where did it happen?** \n\n" +
                $"{MyData.Place} \n\n" +
                $"**7. Are there any other details you remember about the location?** \n\n" +
                $"{MyData.PlaceAdditional} \n\n" +
                $"**8. Did anyone else see or hear either all or any part of what happened?** \n\n" +
                $"{MyData.Eyewitness} \n\n" +
                $"**9. Did you interact with anyone right before or after the incident?** \n\n" +
                $"{MyData.Interact} \n\n" +
                $"**10. What information do you know about any of the people described above?** \n\n" +
                $"{MyData.EyewitnessAdditional} \n\n" +
                $"**11. Did you tell anyone about the incident?** \n\n" +
                $"{MyData.ToldOthers} \n\n" +
                $"**12. What information do you know about the people you told?** \n\n" +
                $"{MyData.ToldOthersAdditional} \n\n" +
                $"**13. Do you know if the offender told anyone about the incident?** \n\n" +
                $"{MyData.Offender} \n\n" +
                $"**14. You also indicated that:** \n\n" +
                $"{MyData.multiplechoice} \n\n" +
                $"**15. What happened? Anything you remember:** \n\n" +
                $"{MyData.remember} \n\n" +
                $"**16. How may offenders were there?** \n\n" +
                $"{MyData.numOfOffender} \n\n" +
                $"**17. What is their name?** \n\n" +
                $"{MyData.nameOfOffender} \n\n" +
                $"**18. Do you know any other information about the offender(s)?** \n\n" +
                $"{MyData.infoOfOffender} \n\n" +
                $"**19. What information do you know about these people?** \n\n" +
                $"{MyData.infoOfThesePeople} \n\n" +
                $"**20. Your attachment uploaded:** \n\n"), cancellationToken);

            var ConfirmResponseChoice = new List<Choice>()
            {
                new Choice() { Value = "Confirm. All information is correct."},
                new Choice() { Value = "Modify 1.Year"},
                new Choice() { Value = "Modify 2.Season"},
                new Choice() { Value = "Modify 3.Date"},
                new Choice() { Value = "Modify 4.Time of day"},
                new Choice() { Value = "Modify 5.What I remember"},
                new Choice() { Value = "Modify 6.Location"},
                new Choice() { Value = "Modify 7.Details about location"},
                new Choice() { Value = "Modify 8.Whether anyone saw it"},
                new Choice() { Value = "Modify 9.Did you interact with anyone"},
                new Choice() { Value = "Modify 10.Information about the people"},
                new Choice() { Value = "Modify 11.Told others"},
                new Choice() { Value = "Modify 12.Information about the people you told"},
                new Choice() { Value = "Modify 13.Whether offender tell others"},
                new Choice() { Value = "Modify 14.Indicated choices"},
                new Choice() { Value = "Modify 15.Anything yyou remember"},
                new Choice() { Value = "Modify 16.Numbers of offenders"},
                new Choice() { Value = "Modify 17.Name of offenders"},
                new Choice() { Value = "Modify 18.Information of offenders"},
                new Choice() { Value = "Modify 19.Information about these people"},
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("Is there any parts that needs to be modified?"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
                Choices = ConfirmResponseChoice,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.multiplechoice), cancellationToken);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.remember), cancellationToken);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.numOfOffender), cancellationToken);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.nameOfOffender), cancellationToken);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.infoOfOffender), cancellationToken);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(MyData.infoOfThesePeople), cancellationToken);
            //return await stepContext.EndDialogAsync();

            // https://stackoverflow.com/questions/58336861/accepting-attachments-on-a-waterfall-dialog-and-storing-them-locally-in-bot-fram
        }

        private async Task<DialogTurnResult> EditingResponse(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Confirm. All information is correct.":
                    EditQuestion = 0;
                    return await stepContext.NextAsync();

                case "Modify 1.Year":
                    EditQuestion = 1;
                    var promptOptions1 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"What year did this happen?"),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = YearGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions1, cancellationToken);

                case "Modify 2.Season":
                    EditQuestion = 2;
                    var promptOptions2 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"What season was it?"),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = SeasonGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions2, cancellationToken);

                case "Modify 3.Date":
                    EditQuestion = 3;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please type your time of day") }, cancellationToken);

                case "Modify 4.Time of day":
                    EditQuestion = 4;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please type your time of day") }, cancellationToken);

                case "Modify 5.What I remember":
                    EditQuestion = 5;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is there anything else you remember about the date? \n Examples: holidays, day of the week, games, school or social events around that time, etc.") }, cancellationToken);

                case "Modify 6.Location":
                    EditQuestion = 6;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Where did it happen?") }, cancellationToken);

                case "Modify 7.Details about location":
                    EditQuestion = 7;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Are there any other details you remember about the location? \n Examples: exact address, area of campus, intersection, building number, neighborhood, buildings or trees nearby, colors you remember, etc.") }, cancellationToken);

                case "Modify 8.Whether anyone saw it":
                    EditQuestion = 8;
                    var promptOptions8 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Did anyone else see or hear either all or any part of what happened?"),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = EyewhitnessGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions8, cancellationToken);

                case "Modify 9.Did you interact with anyone":
                    EditQuestion = 9;
                    var promptOptions9 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Did you interact with anyone right before or after the incident? \n This could be people who were at the scene of the incident or nearby, who saw you or the offender leave, or who helped you call for help."),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = EyewhitnessGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions9, cancellationToken);

                case "Modify 10.Information about the people":
                    EditQuestion = 10;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What information do you know about any of the people described above?  \n Examples: how many people, what they saw or heard, their relationship to you, where they may have been standing or sitting, how physically close to you they were.") }, cancellationToken);

                case "Modify 11.Told others":
                    EditQuestion = 11;
                    var promptOptions11 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Did you tell anyone about the incident"),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = ToldOthersGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions11, cancellationToken);

                case "Modify 12.Information about the people you told":
                    EditQuestion = 12;
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What information do you know about the people you told? \n Who you told, what you told them, when you told them, how you told them (on the phone, in person, over text, etc), their relationship to you or the offender, etc") }, cancellationToken);

                case "Modify 13.Whether offender tell others":
                    EditQuestion = 13;
                    var promptOptions13 = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Do you know if the offender told anyone about the incident?"),
                        RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                        Choices = ToldOthersGetChoice(),
                        Style = ListStyle.HeroCard,
                    };
                    return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions13, cancellationToken);

                case "Modify 14.Indicated choices":
                    EditQuestion = 14;
                    var message = MessageFactory.Text("");
                    message.Attachments = new List<Attachment>() { Cards.DOCMC() };
                    await stepContext.Context.SendActivityAsync(message, cancellationToken);
                    var opts = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
                        }
                    };

                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts);

                case "Modify 15.Anything yyou remember":
                    EditQuestion = 15;
                    var opts15 = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "What happened? Include anything you are able to remember around what you felt, " +
                                "saw, heard, smelled, tasted, or anything you can’t forget about your experience or experiences with the offender(s).",
                        }
                    };
                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts15);

                case "Modify 16.Numbers of offenders":
                    EditQuestion = 16;
                    var opts16 = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "How many offenders were there? It’s okay if you are not sure, just put what you can remember.",
                        }
                    };
                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts16);

                case "Modify 17.Name of offenders":
                    EditQuestion = 17;
                    var opts17 = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "What is their name?/What are their names? (if known)",
                        }
                    };
                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts17);

                case "Modify 18.Information of offenders":
                    EditQuestion = 18;
                    var opts18 = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "Do you know any other information about the offender(s)? Examples: " +
                        "cell phone number, what job they have, if you’ve seen them before, how you know them, any physical characteristics " +
                        "(hair color, identifiable marks, tattoos, clothing, moles or birthmarks), or anything you could not forget about them.",
                        }
                    };

                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts18);

                case "Modify 19.Information about these people":
                    EditQuestion = 19;
                    var opts19 = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Type = ActivityTypes.Message,
                            Text = "What information do you know about these people?",
                        }
                    };
                    // Display a Text Prompt and wait for input
                    return await stepContext.PromptAsync(nameof(TextPrompt), opts19);

            }
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> GetEditingResponse(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Storing the new data / skip if confirmed
            switch (EditQuestion)
            {
                case 0:
                    return await stepContext.NextAsync();
                //return await stepContext.EndDialogAsync();
                case 1:
                    MyData.Year = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 2:
                    MyData.Season = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 3:
                    MyData.Date = ((string)stepContext.Result);
                    break;
                case 4:
                    MyData.Time = ((string)stepContext.Result);
                    break;
                case 5:
                    MyData.DateAdditional = ((string)stepContext.Result);
                    break;
                case 6:
                    MyData.Place = ((string)stepContext.Result);
                    break;
                case 7:
                    MyData.PlaceAdditional = ((string)stepContext.Result);
                    break;
                case 8:
                    MyData.Eyewitness = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 9:
                    MyData.Interact = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 10:
                    MyData.EyewitnessAdditional = ((string)stepContext.Result);
                    break;
                case 11:
                    MyData.ToldOthers = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 12:
                    MyData.ToldOthersAdditional = ((string)stepContext.Result);
                    break;
                case 13:
                    MyData.Offender = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 14:
                    MyData.multiplechoice = ((FoundChoice)stepContext.Result).Value;
                    break;
                case 15:
                    MyData.remember = ((string)stepContext.Result);
                    break;
                case 16:
                    MyData.numOfOffender = ((string)stepContext.Result);
                    break;
                case 17:
                    MyData.nameOfOffender = ((string)stepContext.Result);
                    break;
                case 18:
                    MyData.infoOfOffender = ((string)stepContext.Result);
                    break;
                case 19:
                    MyData.infoOfThesePeople = ((string)stepContext.Result);
                    break;
            }

            //return await stepContext.ReplaceDialogAsync(??, null, cancellationToken);
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] - 3;
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> WaystoHandleRecord(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("How would you like to handle the record? You may:"), cancellationToken);

            var attachments = new List<Attachment>();
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.GetDocHandleRecord().ToAttachment());
            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions
            {
                //Prompt = MessageFactory.Text($"How would you like to handle the record? "),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = HandleResponse(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

        }

        // Anonymous Report
        private async Task<DialogTurnResult> haveIDStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (((FoundChoice)stepContext.Result).Value == "I want to send this record to the University with my identity disclosed.")
            {
                // go to 4 steps later.
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 3;
                return await stepContext.NextAsync();
            }
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Already have case ID?"),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = new List<Choice>()
                    {
                        new Choice() { Value = "Yes"},
                        new Choice() { Value = "No"},
                    },
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> UserExistsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // string userType = (string)stepContext.Values["UserType"];
            string userId = null;

            if (((FoundChoice)stepContext.Result).Value == "Yes"){
                //save data? or not
                await stepContext.Context.SendActivityAsync("OK, the case will remain anonymous.");
                return await stepContext.NextAsync(null, cancellationToken);
            }
            else{
                 do
                    {
                        userId = Repository.RandomString(7);
                    } while (await _cosmosDBClient.CheckNewUserIdAsync(userId, Configuration["CosmosEndPointURI"], Configuration["CosmosPrimaryKey"], Configuration["CosmosDatabaseId"], Configuration["CosmosContainerID"], Configuration["CosmosPartitionKey"]));

                    User.UserID = userId;
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Your Case ID is: " + User.UserID), cancellationToken);
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please make a record of the Case ID so that you can come back and access the case report later when needed."), cancellationToken);
                    return await stepContext.NextAsync(null, cancellationToken);
            }
        }

        //  private async Task<DialogTurnResult> saveStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     var userDetails = (User)stepContext.Options;
        //     stepContext.Values["Task"] = MyData.Date;
        //     userDetails.TasksList.Add((string)stepContext.Values["Task"]);

        //     return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
        //     {
        //         Prompt = MessageFactory.Text("Would you like to Add more tasks?")
        //     }, cancellationToken);
        // }

        private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await _cosmosDBClient.AddItemsToContainerAsync(User.UserID, MyData);
            // var userDetails = (User)stepContext.Result;
            
            // await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please wait while I add your tasks to the database..."), cancellationToken);
            // for (int i = 0; i < userDetails.TasksList.Count; i++)
            // {
            //     if (await _cosmosDBClient.AddItemsToContainerAsync(User.UserID, userDetails.TasksList[i]) == -1)
            //     {
            //         await stepContext.Context.SendActivityAsync(MessageFactory.Text("The Task '" + userDetails.TasksList[i] + "' already exists"), cancellationToken);
                    
            //     }
                
            // }
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Add Task operation completed. Thank you."), cancellationToken);

            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> ShowTasksStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            List<ToDoTask> toDoTasks = await _cosmosDBClient.QueryItemsAsync(User.UserID, Configuration["CosmosEndPointURI"], Configuration["CosmosPrimaryKey"], Configuration["CosmosDatabaseId"], Configuration["CosmosContainerID"], Configuration["CosmosPartitionKey"]);
            if (toDoTasks.Count == 0)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You don't have any tasks added."), cancellationToken);
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please find the below tasks you provided."), cancellationToken);
            //for (int i = 0; i < toDoTasks.Count; i++)
            //{
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(toDoTasks[0].Year), cancellationToken);
            //}

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> StartOfIdentityDisclosed(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (Globals.DEBUG_MODE == 1)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Enter Idneity Disclosed Part"), cancellationToken);
            }
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

        private IList<Choice> HandleResponse()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "I want to remain anonymous"},
                new Choice() { Value = "I want to send this record to the University with my identity disclosed."},
            };

            return cardOptions;
        }
    }
}
