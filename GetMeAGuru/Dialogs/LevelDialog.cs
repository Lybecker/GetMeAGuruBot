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
    public class LevelDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await SelectPath(context);
        }

        private async Task SelectPath(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.List;
            List<Attachment> attachements = new List<Attachment>();
            var actions = new List<CardAction>();
            actions.Add(AttachmentCreation.CreateCardAction("L1", "L1"));
            actions.Add(AttachmentCreation.CreateCardAction("L2", "L2"));
            actions.Add(AttachmentCreation.CreateCardAction("L3", "L3"));
            actions.Add(AttachmentCreation.CreateCardAction("L4", "L4"));
            var attachment = AttachmentCreation.CreateHeroCardAttachment("Select the Knowledge Level", null, null, null, actions);
            attachements.Add(attachment);
            message.Attachments = attachements;
            await context.PostAsync(message);
            context.Wait(ActionSelected);

        }

        public virtual async Task ActionSelected(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
         //   await context.PostAsync(message.Text);
            context.Done(message.Text);
        }
    }
}