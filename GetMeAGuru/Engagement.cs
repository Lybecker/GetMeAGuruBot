using System;

namespace GetMeAGuru
{
    public class Engagement
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public DateTime Date { get; set; }
        public string[] Tech { get; set; }
        public string[] Resources { get; set; }
    }
}