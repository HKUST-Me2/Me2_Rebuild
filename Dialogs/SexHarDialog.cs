using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;


namespace Microsoft.BotBuilderSamples
{
    public class KnowMoreSexHarDialog : ComponentDialog
    {
        public KnowMoreSexHarDialog()
            : base(nameof(KnowMoreSexHarDialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new SexHar_OtherStoryDialog());
            AddDialog(new SexHar_VictimDialog());
            AddDialog(new ByStanderApproachDialog());
            AddDialog(new MiniQuizDialog());
            AddDialog(new OcampDialog());
            AddDialog(new SexHar_ExampleDialog());

        
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStep3Async,
                    Options3Async,
                }));
                InitialDialogId = nameof(WaterfallDialog);

        }
        private async Task<DialogTurnResult> SelectionStep3Async(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-1"), cancellationToken);}

            await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"Any unwelcome demand, conduct of sexual nature, gender discrimination and speech would make others feel offended, insulted, or intimidated." +
                $"Sexual harassment creates a hostile or intimidating environment. It includes any unwanted verbal or physical sexual behaviour ranging from sexual comments about a person’s clothing, anatomy, or looks, to very serious acts that qualify as assault or rape." +
                $"Sexual harassment is not about the intention of the offender and rather is  about the impact and severity of the behaviour on victims."), cancellationToken);
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(Globals.prompt_text),
                RetryPrompt = MessageFactory.Text(Globals.reprompt_text),
                Choices = GetChoices3(),
                Style = ListStyle.HeroCard,
            };

            // Prompt the user for a choice.
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2"), cancellationToken);}
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> Options3Async(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "I want to know others' stories":
                    return await stepContext.BeginDialogAsync(nameof(SexHar_OtherStoryDialog), null, cancellationToken);
                case "Can you give me some examples of sexual harassment?":
                    return await stepContext.BeginDialogAsync(nameof(SexHar_ExampleDialog), null, cancellationToken);
                case "What can I do if I think I'm sexually harassed?":
                    return await stepContext.BeginDialogAsync(nameof(SexHar_VictimDialog), null, cancellationToken);
                case "What can I do if I think I've witnessed sexual harassment?":
                    return await stepContext.BeginDialogAsync(nameof(ByStanderApproachDialog), null, cancellationToken);
                case "I want to know more about issues related to orientation camps":
                    return await stepContext.BeginDialogAsync(nameof(OcampDialog), null, cancellationToken);
                case "I think I am an expert at understanding sexual harassment. Test me!":
                    return await stepContext.BeginDialogAsync(nameof(MiniQuizDialog), null, cancellationToken);
                default:
                    break;
            }
            return await stepContext.EndDialogAsync(); 

        }

        private IList<Choice> GetChoices3()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "I want to know others' stories"},
                new Choice() { Value = "Can you give me some examples of sexual harassment?"},
                new Choice() { Value = "What can I do if I think I'm sexually harassed?"},
                new Choice() { Value = "What can I do if I think I've witnessed sexual harassment?"},
                new Choice() { Value = "I want to know more about issues related to Orientation camps"},
                new Choice() { Value = "I think I am an expert at understanding sexual harassment. Test me!"}

            };

            return cardOptions;
        }
    }
}
