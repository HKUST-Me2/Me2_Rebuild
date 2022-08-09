using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Choices;


namespace Microsoft.BotBuilderSamples
{
    public class MainDialog : ComponentDialog
    {
        private readonly UserState _userState;

        public MainDialog(UserState userState)
            : base(nameof(MainDialog))
        {
            _userState = userState;

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new KnowMoreSexHarDialog());
            AddDialog(new MoreBotDialog());
            AddDialog(new QnADialog());
            AddDialog(new DocumentationDialog());
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
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID=4"),
                cancellationToken);}

            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text(Globals.prompt_text),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = GetChoices(),
                Style = ListStyle.HeroCard,
            };

            // Prompt the user with the configured PromptOptions.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
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
