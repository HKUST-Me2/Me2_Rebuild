using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Azure;
using Azure.AI.Language.QuestionAnswering;
using System;


namespace Microsoft.BotBuilderSamples
{
    public class QnADialog : ComponentDialog
    {
        public QnADialog()
            : base(nameof(QnADialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                QnA,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> QnA(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Entered the QnA maker diaglog"), cancellationToken);

            Uri endpoint = new Uri("https://me2-language.cognitiveservices.azure.com/");
            AzureKeyCredential credential = new AzureKeyCredential("3f186dbcee484afdad0464d6023ac28c");
            string projectName = "Me2";
            string deploymentName = "production";

            string question = "How long should my Surface battery last?";

            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            AnswersOptions options = new AnswersOptions(); //confidence score
            options.ConfidenceThreshold = 0; //confidence score

            Response<AnswersResult> response = client.GetAnswers(question, project, options); //Add the additional options parameter
            

            foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
            {
                await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Q:{question}"), cancellationToken);
                
                await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"A:{answer.Answer}"), cancellationToken);

                await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"({answer.Confidence})"), cancellationToken);
                
                // Console.WriteLine($"Q:{question}");
                // Console.WriteLine($"A:{answer.Answer}");
                // Console.WriteLine($"({answer.Confidence})");
            }

            // Exit the dialog
            return await stepContext.EndDialogAsync();
        }
    }
}