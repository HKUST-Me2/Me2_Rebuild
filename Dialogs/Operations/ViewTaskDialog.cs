﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.BotBuilderSamples.Utilities;

namespace Microsoft.BotBuilderSamples.Dialogs.Operations
{
    public class ViewTaskDialog : ComponentDialog
    {
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        public ViewTaskDialog(IConfiguration configuration, CosmosDBClient cosmosDBClient) : base(nameof(ViewTaskDialog))
        {
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;

            var waterfallSteps = new WaterfallStep[]
            {
                ShowTasksStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            InitialDialogId = nameof(WaterfallDialog);
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
            // for (int i = 0; i < toDoTasks.Count; i++)
            // {
            //     await stepContext.Context.SendActivityAsync(MessageFactory.Text(toDoTasks[i].Task), cancellationToken);
            // }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
