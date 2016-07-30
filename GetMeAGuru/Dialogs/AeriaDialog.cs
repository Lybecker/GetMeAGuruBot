using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace GetMeAGuru.Dialogs
{
    [Serializable()]
    public class AeriaDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            return null;
        } 
    }
}