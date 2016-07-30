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

        private async Task TechnologySelection(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            List<Attachment> attachments = new List<Attachment>();
            foreach (var item in Technologies)
            {
                var actions = new List<CardAction>();
                actions.Add(AttachmentCreation.CreateCardAction("Select this Path", item.ToString()));
                var attachment = AttachmentCreation.CreateHeroCardAttachment(item.ToString(), null, null, "https://azurecomcdn.azureedge.net/cvt-9c42e10c78bceeb8622e49af8d0fe1a20cd9ca9f4983c398d0b356cf822d8844/images/shared/social/azure-icon-250x250.png", actions);
                attachments.Add(attachment);
            }
            message.Attachments = attachments;
            await context.PostAsync(message);
            context.Wait(TechnologySelected);
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