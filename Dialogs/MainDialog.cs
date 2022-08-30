using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Choices;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.BotBuilderSamples.Dialogs.Operations;
using Microsoft.BotBuilderSamples.Utilities;

namespace Microsoft.BotBuilderSamples
{
    public class MainDialog : ComponentDialog
    {
        private readonly ToDoLUISRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly string UserValidationDialogID = "UserValidationDlg";

        public MainDialog(ToDoLUISRecognizer luisRecognizer, ILogger<MainDialog> logger, IConfiguration configuration, CosmosDBClient cosmosDBClient)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new KnowMoreSexHarDialog());
            AddDialog(new MoreBotDialog());
            AddDialog(new QnADialog());
            AddDialog(new DocumentationDialog(_luisRecognizer, Configuration, _cosmosDBClient));
            AddDialog(new ReviewSelectionDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {


            // Create the PromptOptions which contain the prompt and re-prompt messages.
            // PromptOptions also contains the list of choices available to the user.
            if (Globals.DEBUG_MODE == 1)
            {
                await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID=4"),
                cancellationToken);
            }

            if (Globals.CameBack)
            {
                // user came back
                var options = new PromptOptions()
                {
                    Prompt = MessageFactory.Text("Is there anything else that I can help you?"),
                    RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                    Choices = GetChoices(),
                    Style = ListStyle.HeroCard,
                };
                // Prompt the user with the configured PromptOptions.
                return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
            }
            else
            {
                // it's user's first time to see this dialog
                var options = new PromptOptions()
                {
                    Prompt = MessageFactory.Text(Globals.prompt_text),
                    RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                    Choices = GetChoices(),
                    Style = ListStyle.HeroCard,
                };
                Globals.CameBack = true;
                // Prompt the user with the configured PromptOptions.
                return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
            }

            // Prompt the user with the configured PromptOptions.
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {       
            if (Globals.DEBUG_MODE==1){
            await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"Debug: you chose {((FoundChoice)stepContext.Result).Value}"),
            cancellationToken);}

            // First/ Main branch. This triggers other diaglogs.
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "I want to know more about sexual harassment":
                    return await stepContext.BeginDialogAsync(nameof(KnowMoreSexHarDialog), null, cancellationToken);
                case "I want to know more about you":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"entered case 2"),cancellationToken);
                    return await stepContext.BeginDialogAsync(nameof(MoreBotDialog), null, cancellationToken);
                case "I want to record a case":
                    return await stepContext.BeginDialogAsync(nameof(DocumentationDialog), null, cancellationToken);
                    
                case "I just want to play with you":
                    return await stepContext.BeginDialogAsync(nameof(QnADialog), null, cancellationToken);
                    
                default:
                    return await stepContext.EndDialogAsync(); 
            }          
}

        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "I want to know more about sexual harassment"},
                new Choice() { Value = "I want to know more about you"},
                new Choice() { Value = "I want to record a case"},
                new Choice() { Value = "I just want to play with you"},
            };

            return cardOptions;
        }
    }
}
