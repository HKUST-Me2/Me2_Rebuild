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
    public class OcampDialog : ComponentDialog
    {

        public OcampDialog()
            : base(nameof(OcampDialog))
        {

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                OcampExamples,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> OcampExamples(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          
            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2-6-1"), cancellationToken);}
            
            await stepContext.Context.SendActivityAsync(
            MessageFactory.Text("Examples of sexual harassment occurring in orientation activities include:"), cancellationToken);

            await stepContext.Context.SendActivityAsync(
            MessageFactory.Text("1. Spot particular girls who are \"good-looking\" and \"sexy\" and ask for room to stay alone \n"+
                                "2. Be guided to take off their clothes \n"+
                                "3. Punishment of games involve sexual nature \n"+
                                "4. Take advantage of group members by having close physical contact"), cancellationToken);

            if (Globals.DEBUG_MODE==1) {await stepContext.Context.SendActivityAsync(
            MessageFactory.Text($"#ID:4-2-2-6-2"), cancellationToken);}
            
            await stepContext.Context.SendActivityAsync(
            MessageFactory.Text("If you encounter such situation, please SAY NO directly. Make it clear to the harasser that you do not welcome the behaviour and it must stop, for example by yelling at him/her to stop, pushing him/her away, or sending a text message to voice your objection to such behaviour"), cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("⌨️Type anything to go back."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }

    }

}
