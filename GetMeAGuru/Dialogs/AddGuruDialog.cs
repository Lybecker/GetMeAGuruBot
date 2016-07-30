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
            await context.PostAsync("Enter sth");
            var message = context.MakeMessage();
            List<string> tech = new List<string>();
            string location = "";
            string alias = null;
            string company = "";
            string date = "";
            
            foreach (var e in result.Entities)
            {
                if (e.Type == "Technologies")
                {
                    tech.Add(e.Entity);
                } else if (e.Type == "Alias")
                {
                    alias = e.Entity;                    
                }
                else if (e.Type == "builtin.datetime.date")
                {
                    date = e.Resolution["date"];
                }
                else if (e.Type == "builtin.geography.country")
                {
                    location = e.Entity;
                }
                else if (e.Type == "builtin.encyclopedia.organization.organization")
                {
                    company = e.Entity;
                }                
            }

            if (alias != null)
            {
                SearchClient sc = new SearchClient();
                SearchGuru eng = new SearchGuru();
                eng.alias = alias;
                eng.techs.Concat(tech);
                sc.Add(eng);
            }

             message.Text = $"Sounds like a cool engagment! \n\n" 
                + "Alias: " + alias + "\n\n" 
                + "Location: " + location + "\n\n" 
                + "Company: " + company + "\n\n"
                + "Technologies used: " + string.Join(", ", tech) + "\n\n"
                + "Engagment Logged!";

            await context.PostAsync(message.Text);
            context.Wait(MessageReceived);
            context.Done(message.Text);
        }

/*
        private void StoreEngagmentData(LuisResult result)
        {
            foreach(var e in result.Entities)

    {

            }
            
        }*/
    }
}