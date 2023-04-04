
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// MiniQuiz Version

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;


// IDK otherstoriescard.cs and sexHar__MiniQuizDialog.cs has a common part? 

namespace Microsoft.BotBuilderSamples
{
    public class MiniQuizDialog : ComponentDialog
    {
      
        private int randomized;
        private int score;

        //MiniQuiz Questions and answers.
        #region StringDeclaration
        const string miniquizQ1 = "It is not sexual harassment if it was just a one-off incident.";
        const string miniquizQ1Ans = "Under the law, whether an act amounts to sexual harassment has nothing to do with its frequency. " +
                                "An act can constitute sexual harassment even if it happened only once.";
        const string miniquizQ2 = "It is always a “she said, he said” situation. It is difficult to prove sexual harassment, and harassers get off the hook all the time.";
        const string miniquizQ2Ans = "While the claimant of a sexual harassment claim has the burden of proof to support his/her claim with " +
                                "reasons and evidence in legal proceedings, such claims are civil in nature, meaning that the standard of proof " +
                                "adopted by the Court would not be “beyond reasonable doubt” as in criminal cases, but “on the balance of probabilities”. " +
                                "In adopting this standard of proof, the Court would evaluate the evidence presented by both the claimant and the respondent " +
                                "before determining whose account is more credible, and whether to accept the issues raised by the claimant and his/her claim. " +
                                "The standard of proof is therefore considerably lower than that in criminal proceedings.";
        const string miniquizQ3 = "It is not sexual harassment if the harasser did not mean it.";
        const string miniquizQ3Ans = "Intent is not a necessary element of sexual harassment under the law. A person making a sexual joke, " +
                                "for example, may consider it innocuous or a common practice in certain workplaces, but as long as it is " +
                                "unwelcome to the recipient or creates a hostile or intimidating environment for others, " +
                                "the act can amount to sexual harassment.";
        const string miniquizQ4 = "Men cannot be sexually harassed.";
        const string miniquizQ4Ans = "The law protects everyone, regardless of the gender of the victim.";
        const string miniquizQ5 = "Sexual harassment never happens between people of the same sex.";
        const string miniquizQ5Ans = "Sexual harassment can take place between men and women, among men, or among women. " +
                                "Gender is irrelevant as to whether an act amounts to sexual harassment.";
        const string miniquizQ6 = "Some people mean “yes” when they say “no”.";
        const string miniquizQ6Ans = "When a person looks offended, pushes you away, or explicitly says “no” to you, " +
                                "it is a clear sign that he/she does not welcome whatever you are doing. When you are unsure, " +
                                "take the time to communicate with him/her, and always obtain consent before engaging in any behaviour " +
                                "that may otherwise constitute sexual harassment.";
        //MiniQuiz Response for correct answer
        const string miniquizCorrect1 = "Correct!👍";
        const string miniquizCorrect2 = "Congrats! It's Correct!👍👍👍";
        const string miniquizCorrect3 = "You're Right!👍";
        //MiniQuiz Respose for wrong answer
        const string miniquizWrong1 = "Sorry, it's wrong.😢";
        const string miniquizWrong2 = "That's wrong.😢";
        const string miniquizWrong3 = "I don't think you're right about that.😢";
        const string miniquizWrong4 = "I'm afraid you're mistaken.😢";
        //MiniQuiz Response for Next Question
        const string miniquizNext1 = "✨the Next one! 👇";
        const string miniquizNext2 = "✨Here comes the next! 👇";
        const string miniquizNext3 = "✨Let's Continue! 👇";
        const string miniquizNext4 = "✨Keep it up! And the next is ... 👇";
        const string miniquizNextLast = "✨The Last one! 👇";
        //MiniQuiz Response for going to the last question 
        const string miniquizEnding = "😸Thanks for playing our mini quiz!😸";
        #endregion

        public MiniQuizDialog()
            : base(nameof(MiniQuizDialog))
        {
           
            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                FirstQuestion,
                SecondQuestion,
                ThirdQuestion,
                FourthQuestion,
                FifthQuestion,
                SixQuestion,
                LastAnswer,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        /// <summary>
        /// Sending Animated Card to welcome User for playing the miniquiz
        /// </summary>
        private Microsoft.Bot.Schema.IMessageActivity MiniQuizWelcoming()
        {
            // Sending an animated card to welcome player. 
            var MiniQuizCard = new List<Attachment>();                                      // Creating a list of attachments
            var MiniQuizReply = MessageFactory.Attachment(MiniQuizCard);                    // Reply with an activity
            MiniQuizReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;                // Set the card to be a carousel
            MiniQuizReply.Attachments.Add(Cards.GetMiniquizAnimationCard().ToAttachment()); // Add the hero card to be carousel
            return MiniQuizReply;  // Send out the carousel
        }

        /// <summary>
        /// Sending appropriate Question 
        /// </summary>
        /// <param name="stepContext"></param>
        /// <param name="cancellationToken"></param>
        private string MiniQuizQuestionGeneration()
        {
            randomized %= 6; 
            switch (randomized) 
            {
                case 0:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-1"), cancellationToken);}

                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ1), cancellationToken);
                    randomized++;
                    return miniquizQ1;

                case 1:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-5"), cancellationToken);}

                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ2), cancellationToken);
                    randomized++;
                    return miniquizQ2;

                case 2:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-9"), cancellationToken);}

                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ3), cancellationToken);
                    randomized++;
                    return miniquizQ3;

                case 3:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-13"), cancellationToken);}

                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ4), cancellationToken);
                    randomized++;
                    return miniquizQ4;

                case 4:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-17"), cancellationToken);}

                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ5), cancellationToken);
                    randomized++;
                    return miniquizQ5;


                // IDK #ID:4-2-2-5-21,22,23,24 is missing  
                case 5:
                    //if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
                    //MessageFactory.Text($"#ID:4-2-2-5-5-25"), cancellationToken);}
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ6), cancellationToken);
                    randomized++;
                    return miniquizQ6;

                default:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text("Wrong number lol"), cancellationToken);
                    randomized++;
                    return "Wrong Randomized Number";

            }
            //randomized++;
        }

    private string MiniQuizAnswerGeneration()
    {
            //randomized %= 6;
            switch ((randomized-1)%6) 
            {
                case 0:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-1"), cancellationToken);
            }

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ1), cancellationToken);
            return miniquizQ1Ans;

                case 1:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-5"), cancellationToken);
            }

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ2), cancellationToken);
            return miniquizQ2Ans;

                case 2:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-9"), cancellationToken);
            }

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ3), cancellationToken);
            return miniquizQ3Ans;

                case 3:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-13"), cancellationToken);
            }

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ4), cancellationToken);
            return miniquizQ4Ans;

                case 4:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-17"), cancellationToken);
            }

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ5), cancellationToken);
            return miniquizQ5Ans;


                // IDK #ID:4-2-2-5-21,22,23,24 is missing  
                case 5:
                    if (Globals.DEBUG_MODE == 1)
            {
                //await stepContext.Context.SendActivityAsync(
                //    MessageFactory.Text($"#ID:4-2-2-5-5-25"), cancellationToken);
            }
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizQ6), cancellationToken);
            return miniquizQ6Ans;

            default:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text("Wrong number lol"), cancellationToken);
                    return "Wrong Randomized Number";

        }
    }

    private async Task<DialogTurnResult> FirstQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            // Some initialization for miniquiz Part.
            score = 0;
            await stepContext.Context.SendActivityAsync(MiniQuizWelcoming(), cancellationToken);
            //MiniQuizWelcoming(stepContext,cancellationToken);       // Welcoming user
            Random rnd = new Random();                              // Sending Miniquiz Questions in a simple randomized order. 
            randomized = rnd.Next(6);                               // Randomize an integer, while 0 <= randomized < 6
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> SecondQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);
            // Sending response of "NEXT" + "Question"
            //MiniQuiz_NextMsg(stepContext, cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_NextMsg()), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> ThirdQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           // _logger.LogInformation("MainDialog.ThirdQuestion");

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);
            // Sending response of "NEXT" + "Question"
            //MiniQuiz_NextMsg(stepContext, cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_NextMsg()), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> FourthQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("MainDialog.FourthQuestion");

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);
            // Sending response of "NEXT" + "Question"
            //MiniQuiz_NextMsg(stepContext, cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_NextMsg()), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> FifthQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("MainDialog.FifthQuestion");

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);
            // Sending response of "NEXT" + "Question"
            //MiniQuiz_NextMsg(stepContext, cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_NextMsg()), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> SixQuestion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("MainDialog.SixQuestion");

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);
            // Sending response of "NEXT" + "Question"
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNextLast), cancellationToken);
            //MiniQuizQuestionGeneration(stepContext, cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizQuestionGeneration()), cancellationToken);

            // A List of Yes/ No, which can be chosen by user.
            var MiniQuizYesNo = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
            };
            // Creating a hero card
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("🤔Is this statement correct?"),
                RetryPrompt = MessageFactory.Text("Oops, I'm afraid I don't speak that language! How about we try something else, or perhaps choose from one of the options I provided?"),
                Choices = MiniQuizYesNo,
                Style = ListStyle.HeroCard,
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        private async Task<DialogTurnResult> LastAnswer(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           // _logger.LogInformation("MainDialog.LastAnswer");

            // Giving response for first questions' answer (i.e. correct or not)
            if (((FoundChoice)stepContext.Result).Value == "Yes") 
            {
                // Wrong Answer
                //MiniQuiz_WrongMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_WrongMsg()), cancellationToken);
            }
            else
            {
                // Correct Answer
                //MiniQuiz_CorrectMsg(stepContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuiz_CorrectMsg()), cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(MiniQuizAnswerGeneration()), cancellationToken);

            // print out score
            if (score <= 3 )
            {
                // correct more than half
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Opps, your score is "+ score +". You can definitely do better! Go learn more about sexual harassment and try again later to see if you get a higher score!"), cancellationToken);

            }
            else if (score <= 5)
            {
                // correct less than half
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Not bad! Your score is " + score + ".You can be an expert if you learn a bit more about sexual harassment!"), cancellationToken);
                // direct to the education part

            }
            else
            {
                // score >= 6 
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Congrats! Your score is " + score + ". You did a great job. Keep it up!!"), cancellationToken);
                // direct to the education part 
            }

            // Sending response of Thanks"
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizEnding), cancellationToken);

            // await stepContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);


            // return await stepContext.EndDialogAsync();
            return await stepContext.ReplaceDialogAsync(nameof(MainDialog), null, cancellationToken);
        }

        private string MiniQuiz_CorrectMsg()
        {
            const string miniquizCorrect1 = "Correct!👍";
            const string miniquizCorrect2 = "Congrats! It's Correct!👍👍👍";
            const string miniquizCorrect3 = "You're Right!👍";

            score++;

            Random rnd = new Random();
            // Randomize an integer, while 0 <= CorrectRnd < 3
            int CorrectRnd = rnd.Next(3);
            switch (CorrectRnd)
            {
                case 0:
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect1), cancellationToken);
                return miniquizCorrect1;

                case 1:
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect2), cancellationToken);
                return miniquizCorrect2;
 
                default:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizCorrect3), cancellationToken);
                    return miniquizCorrect3;
     
            }
        }

        private string MiniQuiz_WrongMsg()
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
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong1), cancellationToken);
                return miniquizWrong1;
                 
                case 1:
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong2), cancellationToken);
                return miniquizWrong2;
                
                case 2:
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong3), cancellationToken);
                return miniquizWrong3;
                   
                default:
                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizWrong4), cancellationToken);
                return miniquizWrong4;
                 
            }
        }

        private string MiniQuiz_NextMsg()
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
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext1), cancellationToken);
                    return miniquizNext1;
                    
                case 1:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext2), cancellationToken);
                    return miniquizNext2;
                  
                case 2:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext3), cancellationToken);
                    return miniquizNext3;
               
                default:
                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(miniquizNext4), cancellationToken);
                    return miniquizNext4;
                  
            }
        }

        
    }
}