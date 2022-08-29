// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.11.1

using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.BotBuilderSamples.Dialogs.Operations;
using Microsoft.BotBuilderSamples.Utilities;

namespace Microsoft.BotBuilderSamples
{
    public class AddCaseDialog : ComponentDialog
    {
        private readonly ToDoLUISRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly string UserValidationDialogID = "UserValidationDlg";


        // Dependency injection uses this constructor to instantiate MainDialog
        public AddCaseDialog(ToDoLUISRecognizer luisRecognizer, IConfiguration configuration, CosmosDBClient cosmosDBClient)
            : base(nameof(AddCaseDialog))
        {
            _luisRecognizer = luisRecognizer;
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;


            AddDialog(new TextPrompt(nameof(TextPrompt)));
            // AddDialog(new TextPrompt(UserValidationDialogID, UserValidation));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new CreateTaskDialog(_cosmosDBClient));
            AddDialog(new ViewTaskDialog(Configuration, _cosmosDBClient));
            AddDialog(new DeleteTaskDialog(_cosmosDBClient));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                haveIDStepAsync,
                UserExistsStepAsync,
                saveStepAsync,
                SummaryStepAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> haveIDStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
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
            string userType = (string)stepContext.Values["UserType"];
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

         private async Task<DialogTurnResult> saveStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userDetails = (User)stepContext.Options;
            stepContext.Values["Task"] = (string)stepContext.Result;
            userDetails.TasksList.Add((string)stepContext.Values["Task"]);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
            {
                Prompt = MessageFactory.Text("Would you like to Add more tasks?")
            }, cancellationToken);
        }

          private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userDetails = (User)stepContext.Result;
            
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please wait while I add your tasks to the database..."), cancellationToken);
            for (int i = 0; i < userDetails.TasksList.Count; i++)
            {
                // if (await _cosmosDBClient.AddItemsToContainerAsync(User.UserID, userDetails.TasksList[i]) == -1)
                // {
                //     await stepContext.Context.SendActivityAsync(MessageFactory.Text("The Task '" + userDetails.TasksList[i] + "' already exists"), cancellationToken);
                    
                // }
                
            }
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Add Task operation completed. Thank you."), cancellationToken);

            return await stepContext.EndDialogAsync(userDetails, cancellationToken);
        }

    }
}
