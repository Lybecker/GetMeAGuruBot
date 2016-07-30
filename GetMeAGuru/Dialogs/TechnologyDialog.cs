using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using GetMeAGuru.Helpers;
using Microsoft.Bot.Connector;

namespace GetMeAGuru.Dialogs
{
    [Serializable()]
    public class TechnologyDialog : IDialog<string>
    {
        public List<string> Technologies = new List<string>()
    { "Windows Development", "Office365", "Azure", "DevOps", "Game Dev", "Web Dev", "Machine Learning", "IoT", "Media", "High Scale Data" };

        public async Task StartAsync(IDialogContext context)
        {
            await TechnologySelection(context);
        }

        Attachment attachment;

        private async Task TechnologySelection(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            List<Attachment> attachements = new List<Attachment>();
            foreach (var item in Technologies)
            {
                var actions = new List<CardAction>();
                actions.Add(AttachmentCreation.CreateCardAction(item.ToString(), item.ToString()));
                attachment = AttachmentCreation.CreateHeroCardAttachment(item.ToString(), null, null, null, actions);
                attachements.Add(attachment);
            }
            message.Attachments = attachements;
            await context.PostAsync("Select a technology path");
            await context.PostAsync(message);

            //var message = context.MakeMessage();
            //message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //List<Attachment> attachements = new List<Attachment>();
            //var actions = new List<CardAction>();
            //foreach (var item in Technologies)
            //{
            //    actions.Add(AttachmentCreation.CreateCardAction("Select this Path", item.ToString()));
            //    var attachment = AttachmentCreation.CreateHeroCardAttachment(item.ToString(), null, null, null, actions);
            //    attachements.Add(attachment);
            //}
            //message.Attachments = attachements;
            //await context.PostAsync("Select a technology path");
            //await context.PostAsync(message);
            //context.Wait(TechnologySelected);
        }

        public virtual async Task TechnologySelected(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            bool found = false;
            foreach (var item in Technologies)
            {
                if (message.Text == item.ToString())
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                context.Done(message.Text);
            }
            else
            {
                await context.PostAsync("Please select an available technology path");
                await TechnologySelection(context);
            }

        }
    }
}