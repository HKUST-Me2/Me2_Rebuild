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
    public class AccessCaseDialog : ComponentDialog
    {
        private readonly ToDoLUISRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly string UserValidationDialogID = "UserValidationDlg";


        // Dependency injection uses this constructor to instantiate MainDialog
        public AccessCaseDialog(ToDoLUISRecognizer luisRecognizer, IConfiguration configuration, CosmosDBClient cosmosDBClient)
            : base(nameof(AccessCaseDialog))
        {
            _luisRecognizer = luisRecognizer;
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;

            AddDialog(new InputDialog(luisRecognizer, configuration,cosmosDBClient));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new TextPrompt(UserValidationDialogID, UserValidation));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new CreateTaskDialog(_cosmosDBClient));
            AddDialog(new ViewTaskDialog(Configuration, _cosmosDBClient));
            AddDialog(new DeleteTaskDialog(_cosmosDBClient));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                UserIDStepAsync,
                UserChooseOperation,

            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> UserIDStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.PromptAsync(UserValidationDialogID, new PromptOptions
            {
                Prompt = MessageFactory.Text("Please enter your user id.")
            }, cancellationToken);
            
        }
        private async Task<DialogTurnResult> UserChooseOperation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.BeginDialogAsync(nameof(InputDialog), null, cancellationToken);

        }

        private async Task<bool> UserValidation(PromptValidatorContext<string> promptcontext, CancellationToken cancellationtoken)
        {
            string userId = promptcontext.Recognized.Value;
            await promptcontext.Context.SendActivityAsync("Please wait, while I validate your details...", cancellationToken: cancellationtoken);

            if (await _cosmosDBClient.CheckNewUserIdAsync(userId, Configuration["CosmosEndPointURI"], Configuration["CosmosPrimaryKey"], Configuration["CosmosDatabaseId"], Configuration["CosmosContainerID"], Configuration["CosmosPartitionKey"]))
            {
                await promptcontext.Context.SendActivityAsync("Your details are verified.", cancellationToken: cancellationtoken);
                User.UserID = userId;
                return true ;
            }
            await promptcontext.Context.SendActivityAsync("The user id you entered is not found, please enter your user id.", cancellationToken: cancellationtoken);
            return false;
        }
    }
}
