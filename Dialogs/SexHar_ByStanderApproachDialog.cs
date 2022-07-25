// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    public class ByStanderApproachDialog : ComponentDialog
    {

        public ByStanderApproachDialog()
            : base(nameof(ByStanderApproachDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ByStanderChooseAction,
                ByStanderProvideSuggestions,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        /// <summary>
        /// Requesting users to choose option in a hero card
        /// (i.e. seeing someone need help OR someone ask for help)
        /// </summary>
        /// <param name="stepContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<DialogTurnResult> ByStanderChooseAction(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2-4-1/#ID:4-2-2-4-2"), cancellationToken);}

            // A List of options, which can be chosen by user.
            var ByStanderChooseCardOptions = new List<Choice>()
            {
                new Choice() { Value = "Seeing someone being sexually harassed", Synonyms = new List<string>() { "Seeing" } },
                new Choice() { Value = "Someone asks you for help", Synonyms = new List<string>() { "Ask" } },
            };

            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("👀Are you seeing someone who is being sexually harassed" +
                                            " or is there someone who asks you for help ?"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select an option from the card."),
                Choices = ByStanderChooseCardOptions,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);

        }

        private async Task<DialogTurnResult> ByStanderProvideSuggestions(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text((((FoundChoice)stepContext.Result).Value)), cancellationToken);

            if (((FoundChoice)stepContext.Result).Value == "Seeing someone being sexually harassed")
            {
                if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID:4-2-2-4-2-2"), cancellationToken);}

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Confront the harasser if circumstances allow," +
                        " and point out what he/she is doing is inappropriate and may amount to sexual harassment."), cancellationToken);
            }
            else  // It is "Ask"
            {
                // Creating a carousel if someone asks user for help.
                var ByStanderAskAttachment = new List<Attachment>();
                // Reply to the activity we received with an activity.
                var ByStanderAskReply = MessageFactory.Attachment(ByStanderAskAttachment);
                ByStanderAskReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"#ID:ID:4-2-2-4-2-2"), cancellationToken);}
                
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk1Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk2Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk3Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk4Card().ToAttachment());
                ByStanderAskReply.Attachments.Add(Cards.GetByStanderAsk5Card().ToAttachment());
                // Send the card(s) to the user as an attachment to the activity
                await stepContext.Context.SendActivityAsync(ByStanderAskReply, cancellationToken);
            }
            return await stepContext.EndDialogAsync();
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.
//        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("MainDialog.ShowCardStepAsync");
//
//
//            // The following is about miniauiz
//            // Some string  used for miniquiz

//
//            // Sending an animated card to welcome player. 
//            var MiniQuizCard = new List<Attachment>();                                      // Creating a list of attachments
//            var MiniQuizReply = MessageFactory.Attachment(MiniQuizCard);                    // Reply with an activity
//            MiniQuizReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;                // Set the card to be a carousel
//            MiniQuizReply.Attachments.Add(Cards.GetMiniquizAnimationCard().ToAttachment()); // Add the hero card to be carousel
//            await stepContext.Context.SendActivityAsync(MiniQuizReply, cancellationToken);  // Send out the carousel
//
//            MiniQuiz_CorrectMsg(stepContext, cancellationToken);
//            MiniQuiz_WrongMsg(stepContext, cancellationToken);
//            MiniQuiz_NextMsg(stepContext, cancellationToken);
//
//
//            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ1), cancellationToken);
//
//            // Sending Miniquiz answers in a simple randomized order. 
//            // Randomize an integer, while 0 <= randomized < 6
//            Random rnd = new Random();
//            int randomized = rnd.Next(6);
//            // make sure 6 questions is asked
//            for(int counter = 0; counter < 6; counter++)
//            {
//                randomized %= 6;
//                switch (randomized)
//                {
//                    case 0:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ1), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    case 1:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ2), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    case 2:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ3), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    case 3:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ4), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    case 4:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ5), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    case 5:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ6), cancellationToken);
//                        // [TODO] Get users response and output yes or no
//                        break;
//                    default:
//                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Wrong number lol"), cancellationToken);
//                        break;
//                }
//                randomized++;
//            }
//
//
//
//
//            // Decide which type of card(s) we are going to show the user
//            /*
//             switch (((FoundChoice)stepContext.Result).Value)
//             {
//                 case "Adaptive Card":
//                     // Display an Adaptive Card
//                     reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
//                     break;
//                 case "Animation Card":
//                     // Display an AnimationCard.
//                     reply.Attachments.Add(Cards.GetAnimationCard().ToAttachment());
//                     break;
//                 case "Audio Card":
//                     // Display an AudioCard
//                     reply.Attachments.Add(Cards.GetAudioCard().ToAttachment());
//                     break;
//                 case "Hero Card":
//                     // Display a HeroCard.
//                     reply.Attachments.Add(Cards.GetHeroCard().ToAttachment());
//                     break;
//                 case "OAuth Card":
//                     // Display an OAuthCard
//                     reply.Attachments.Add(Cards.GetOAuthCard().ToAttachment());
//                     break;
//                 case "Receipt Card":
//                     // Display a ReceiptCard.
//                     reply.Attachments.Add(Cards.GetReceiptCard().ToAttachment());
//                     break;
//                 case "Signin Card":
//                     // Display a SignInCard.
//                     reply.Attachments.Add(Cards.GetSigninCard().ToAttachment());
//                     break;
//                 case "Thumbnail Card":
//                     // Display a ThumbnailCard.
//                     reply.Attachments.Add(Cards.GetThumbnailCard().ToAttachment());
//                     break;
//                 case "Video Card":
//                     // Display a VideoCard
//                     reply.Attachments.Add(Cards.GetVideoCard().ToAttachment());
//                     break;
//                 case "ByStander":
//                     reply.Attachments.Add(Cards.GetByStanderChoiceCard().ToAttachment());
//                     break;
//                 default:
//                     // Display a carousel of all the rich card types.
//                     reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
//                     reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
//                     reply.Attachments.Add(Cards.GetAnimationCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetAudioCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetHeroCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetOAuthCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetReceiptCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetSigninCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetThumbnailCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetVideoCard().ToAttachment());
//                     reply.Attachments.Add(Cards.GetByStanderChoiceCard().ToAttachment());
//                     break;
//             }
//            */
//
//            // Send the card(s) to the user as an attachment to the activity
//            //await stepContext.Context.SendActivityAsync(reply, cancellationToken);
//
//            // Give the user instructions about what to do next
//            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another card."), cancellationToken);
//            await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizEnding), cancellationToken);
//
//            return await stepContext.EndDialogAsync();
//        }
//
        private async void MiniQuiz_CorrectMsg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            const string miniquizCorrect1 = "Correct!👍";
            const string miniquizCorrect2 = "Congrats! It's Correct!👍👍👍";
            const string miniquizCorrect3 = "You're Right!👍";

            Random rnd = new Random();
            // Randomize an integer, while 0 <= CorrectRnd < 3
            int CorrectRnd = rnd.Next(3);
            switch (CorrectRnd)
            {
                case 0:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect1), cancellationToken);
                    break;
                case 1:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect2), cancellationToken);
                    break;
                default:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect3), cancellationToken);
                    break;
            }
        }

        private async void MiniQuiz_WrongMsg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            const string miniquizWrong1 = "Sorry, it's wrong.😢";
            const string miniquizWrong2 = "That's wrong.😢";
            const string miniquizWrong3 = "I don't think you're right about that.😢";
            const string miniquizWrong4 = "I'm afraid you're mistaken.😢";

            Random rnd = new Random();
            // Randomize an integer, while 0 <= CorrectRnd < 3
            int WrongRnd = rnd.Next(4);
            switch (WrongRnd)
            {
                case 0:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong1), cancellationToken);
                    break;
                case 1:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong2), cancellationToken);
                    break;
                case 2:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong3), cancellationToken);
                    break;
                default:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong4), cancellationToken);
                    break;
            }
        }

        private async void MiniQuiz_NextMsg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            const string miniquizNext1 = "✨the Next one! 👇";
            const string miniquizNext2 = "✨Here comes the next! 👇";
            const string miniquizNext3 = "✨Let's Continue! 👇";
            const string miniquizNext4 = "✨Keep it up! And the next is ... 👇";

            Random rnd = new Random();
            // Randomize an integer, while 0 <= CorrectRnd < 3
            int NextRnd = rnd.Next(4);
            switch (NextRnd)
            {
                case 0:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext1), cancellationToken);
                    break;
                case 1:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext2), cancellationToken);
                    break;
                case 2:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext3), cancellationToken);
                    break;
                default:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext4), cancellationToken);
                    break;
            }
        }


        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Adaptive Card", Synonyms = new List<string>() { "adaptive" } },
                new Choice() { Value = "Animation Card", Synonyms = new List<string>() { "animation" } },
                new Choice() { Value = "Audio Card", Synonyms = new List<string>() { "audio" } },
                new Choice() { Value = "Hero Card", Synonyms = new List<string>() { "hero" } },
                new Choice() { Value = "OAuth Card", Synonyms = new List<string>() { "oauth" } },
                new Choice() { Value = "Receipt Card", Synonyms = new List<string>() { "receipt" } },
                new Choice() { Value = "Signin Card", Synonyms = new List<string>() { "signin" } },
                new Choice() { Value = "Thumbnail Card", Synonyms = new List<string>() { "thumbnail", "thumb" } },
                new Choice() { Value = "Video Card", Synonyms = new List<string>() { "video" } },
                new Choice() { Value = "ByStander", Synonyms = new List<string>() { "ByStander" } },
                new Choice() { Value = "All cards", Synonyms = new List<string>() { "all" } },
            };

            return cardOptions;
        }
    }
}
