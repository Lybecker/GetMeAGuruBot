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
    public class QueryDialog : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
            await SelectAction(context);
        }

        private async Task SelectAction(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            List<Attachment> attachements = new List<Attachment>();
            var actions = new List<CardAction>();
            actions.Add(AttachmentCreation.CreateCardAction("Search for a Guru", "Search for a Guru"));
            actions.Add(AttachmentCreation.CreateCardAction("Add Engagement", "Add Engagement"));
            var attachment = AttachmentCreation.CreateHeroCardAttachment("How can I help you today?", null, null, null, actions);
            attachements.Add(attachment);
            message.Attachments = attachements;
            await context.PostAsync(message);
            context.Wait(ActionSelected);
        }

        public virtual async Task ActionSelected(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if(message.Text== "Add Engagement")
            {
                await context.PostAsync("Tell me about your engagement. \n\nType something like:  \n\nMy alias is orzohar, I've been working on iot and Azure in Italy with IBM in a hackfest on January 12th 2001");
                message = await argument;
            }
            context.Done(message.Text);
        }

    }
}