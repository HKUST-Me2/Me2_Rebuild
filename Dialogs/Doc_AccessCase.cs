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
            AddDialog(new ResponseDialog(_luisRecognizer, Configuration, _cosmosDBClient));
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
            data MyData = new data();
            //User.UserID = "HL5PQZX"; // hard code this for testing 
            List<ToDoTask> toDoTasks = await _cosmosDBClient.QueryItemsAsync(User.UserID, Configuration["CosmosEndPointURI"], Configuration["CosmosPrimaryKey"], Configuration["CosmosDatabaseId"], Configuration["CosmosContainerID"], Configuration["CosmosPartitionKey"]);
            if (toDoTasks.Count == 0)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You don't have any tasks added."), cancellationToken);
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            //Storing the accessed data to the MyData Class
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
            
            // For Debug: Showing all accessed responses
/*
            if (Globals.DEBUG_MODE == 1)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Debug Response:"),cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(
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

            }
*/
            return await stepContext.BeginDialogAsync(nameof(ResponseDialog), MyData, cancellationToken);

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
