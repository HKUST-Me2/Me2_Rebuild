using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
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
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                QnAGetQuestion,
                QnAProcess,
                QnARepeat,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> QnAGetQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // await stepContext.Context.SendActivityAsync(
            //     MessageFactory.Text($"Entered the QnA maker diaglog"), cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Whats your question?") }, cancellationToken);
        }
        private async Task<DialogTurnResult> QnAProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            Uri endpoint = new Uri("https://me2-language.cognitiveservices.azure.com/");
            AzureKeyCredential credential = new AzureKeyCredential("3f186dbcee484afdad0464d6023ac28c");
            string projectName = "Me2";
            string deploymentName = "production";

            string question = ((string)stepContext.Result);
            //string question = "how are you";

            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            AnswersOptions options = new AnswersOptions(); //confidence score
            options.ConfidenceThreshold = 0; //confidence score

            Response<AnswersResult> response = client.GetAnswers(question, project, options); //Add the additional options parameter
            

            foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
            {
                // await stepContext.Context.SendActivityAsync(
                // MessageFactory.Text($"Q:{question}"), cancellationToken);
                
                await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"{answer.Answer}"), cancellationToken);

                // await stepContext.Context.SendActivityAsync(
                // MessageFactory.Text($"({answer.Confidence})"), cancellationToken);
            }

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(Globals.prompt_text),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = GetQnAChoices(),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }



        private async Task<DialogTurnResult> QnARepeat(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Ask another question":
                    return await stepContext.BeginDialogAsync(nameof(QnADialog), null, cancellationToken);
                case "Quit QnA dialog":
                default:
                    break;
            }

            // Exit the dialog
            return await stepContext.EndDialogAsync();
        }

        private IList<Choice> GetQnAChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Ask another question"},
                new Choice() { Value = "Quit QnA dialog"},
            };

            return cardOptions;
        }
    }
}