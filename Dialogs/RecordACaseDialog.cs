using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.BotBuilderSamples
{
    public class RecordCaseDialog : ComponentDialog
    {
        public RecordCaseDialog()
            : base(nameof(RecordCaseDialog))
        {
            AddDialog(new TextPrompt(nameof(RecordCase))); 
            InitialDialogId = nameof(RecordCase);
        }

        private async Task<DialogTurnResult>RecordCase(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Entered the Record Case diaglog"),
                cancellationToken);

            // Exit the dialog
            return await stepContext.EndDialogAsync();
        }
    }
}