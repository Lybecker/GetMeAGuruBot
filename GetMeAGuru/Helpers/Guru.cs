using Newtonsoft.Json;

namespace GetMeAGuru
{
    public class Guru
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string alias { get; set; }
        public expertises[] topic { get; set; }
        public engagements[] session { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class expertises
    {
        public string title { get; set; }
    }
}
public class engagements
{
    public string title { get; set; }
    public string description { get; set; }
    public string location { get; set; }
    public string company { get; set; }
    public string date { get; set; }
    public tech[] domain { get; set; }
    public resources[] url { get; set; }
}

public class tech
{
    public string domain { get; set; }
}

public class resources
{
    public string url { get; set; }
}
