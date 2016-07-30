using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace GetMeAGuru.Dialogs
{
    [Serializable()]
    public class IntroDialogChain : IDialog<object>
    {
        string selectedTechnologyPath;
        string selectedLevel;
        string selectedArea;
        string selectedAction;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<string>(conversationStarted);
        }
        bool selected = false;
        private async Task conversationStarted(IDialogContext context, IAwaitable<string> s)
        {
            await context.PostAsync("Welcome! I am the 'Get a Guru' bot. How can I help you?");
            var dialog = Chain.From(() => new QueryDialog())
                .ContinueWith<string, string>
                (async (ctx, Action) =>
                {
                    selectedAction = await Action;
                    if (selectedAction == "Search for a Guru")
                    {
                        return new TechnologyDialog();
                    }
                    else if (selectedAction == "Add Engagement")
                    {
                        return new AddGuruDialog();
                    }
                    else
                        return Chain.Return(selectedTechnologyPath);

                }).ContinueWith<string, string>
                (async (ctx, TechnologyPath) =>
                {
                    selectedTechnologyPath = await TechnologyPath;
                    return new LevelDialog();
                }).ContinueWith<string, string>
                (async (ctx, Level) =>
                {
                    selectedLevel = await Level;
                    return new AreaDialog();
                }).ContinueWith<string, string>
                (async (ctx, Aeria) =>
                {
                    selectedArea = await Aeria;
                    return Chain.Return(selectedArea);
                });
            context.Call(dialog, ProcessFinished);

        }

        private async Task ProcessFinished(IDialogContext ctx, IAwaitable<string> dt)
        {
            await ctx.PostAsync($"Technology: {selectedAction} \n\nLevel: {selectedLevel} \n\nArea: {selectedArea}");
            var message = await dt;
            PromptDialog.Confirm(ctx, ConfirmAsync, "Are these what you are looking for?", "Sorry, didn't get that", 3, PromptStyle.PerLine);       
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync("Available TEs:");
                var client = new SearchClient();
                var result = client.Search(selectedAction);
                string results=null;
                foreach (var item in result)
                {
                   results+=($"\n\n{item.alias}@microsoft.com");
                }
                await context.PostAsync(results);
            }
            else
                await context.PostAsync("OK");
            context.Done("Over");
        }
    }
}