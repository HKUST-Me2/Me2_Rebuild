using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples
{
    public class DocumentationDialog : ComponentDialog
    {
        public DocumentationDialog() :
            base(nameof(DocumentationDialog))
        {
            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new AccessCaseDialog());
            AddDialog(new RecordCaseDialog());

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
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
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