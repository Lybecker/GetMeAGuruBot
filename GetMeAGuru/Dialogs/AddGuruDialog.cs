using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;

namespace GetMeAGuru.Dialogs
{
    [LuisModel("29ed0af1-c4d4-4c79-ab48-20e0bfa765b5", "bfb7c47fa6b84ed8b71a134b73c47ced")]
    [Serializable()]
    public class AddGuruDialog : LuisDialog<string>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Got these: " + string.Join(", ", result.Entities.Select(i => i.Entity));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}