using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.BotBuilderSamples
{
    public class QnADialog : ComponentDialog
    {
        public QnADialog()
            : base(nameof(QnADialog))
        {
            AddDialog(new TextPrompt(nameof(QnA))); 
            InitialDialogId = nameof(QnA);
        }

        private async Task<DialogTurnResult>QnA(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Entered the QnA maker diaglog"),
                cancellationToken);

            // Exit the dialog
            return await stepContext.EndDialogAsync();
        }
    }
}