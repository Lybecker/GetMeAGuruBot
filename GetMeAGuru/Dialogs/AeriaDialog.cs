using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using GetMeAGuru.Helpers;

namespace GetMeAGuru.Dialogs
{
    [Serializable()]
    public class AeriaDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await SelectPath(context);
        }

        private async Task SelectPath(IDialogContext context)
        {
            var message = context.MakeMessage();
            await context.PostAsync("Please enter the city/location you want");
            context.Wait(ActionSelected);

        }

        public virtual async Task ActionSelected(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            context.Done(message.Text);
        }
    }
}