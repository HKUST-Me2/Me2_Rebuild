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
    public class DocumentationDialog : ComponentDialog
    {
        private readonly ToDoLUISRecognizer _luisRecognizer;
        protected readonly IConfiguration Configuration;
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly string UserValidationDialogID = "UserValidationDlg";
        public DocumentationDialog(ToDoLUISRecognizer luisRecognizer, IConfiguration configuration, CosmosDBClient cosmosDBClient)
             : base(nameof(DocumentationDialog))
        {
            _luisRecognizer = luisRecognizer;
            Configuration = configuration;
            _cosmosDBClient = cosmosDBClient;

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new AccessCaseDialog(_luisRecognizer, Configuration, _cosmosDBClient));
            AddDialog(new RecordCaseDialog(_luisRecognizer, Configuration, _cosmosDBClient));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog),
                new WaterfallStep[] {
                    DocumentationChooseAction,
                    DocumentationGetAction
                }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DocumentationChooseAction(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("Please choose the action:"),
                // RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = DocumentationGetChoices(),
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> DocumentationGetAction(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Record Case":
                    return await stepContext.BeginDialogAsync(nameof(RecordCaseDialog), null, cancellationToken);
                case "Access Case":
                    return await stepContext.BeginDialogAsync(nameof(AccessCaseDialog), null, cancellationToken);
                default:
                    return await stepContext.EndDialogAsync();
            }
        }

        private IList<Choice> DocumentationGetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Record Case"},
                new Choice() { Value = "Access Case"},
            };

            return cardOptions;
        }



    }
}