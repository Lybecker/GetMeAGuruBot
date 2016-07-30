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
                        return null;
                }).ContinueWith<string, string>
                (async (ctx, TechnologyPath) =>
                {
                    selectedTechnologyPath = await TechnologyPath;
                    return new LevelDialog();
                }).ContinueWith<string, string>
                (async (ctx, Level) =>
                {
                    selectedLevel = await Level;
                    return new AeriaDialog();
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
            await ctx.PostAsync($"{selectedAction} {selectedArea} {selectedTechnologyPath} {selectedLevel}");
            var client = new SearchClient();

            client.Search(selectedTechnologyPath);
            ctx.Done("Over");
        }
    }
}