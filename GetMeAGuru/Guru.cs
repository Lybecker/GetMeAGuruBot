public class Guru
{
    public string id { get; set; }
    public string alias { get; set; }
    public Expertise expertise { get; set; }
    public Engagment[] engagments { get; set; }
}

public class Expertise
{
    public int WindowsDevelopment { get; set; }
    public int Office365 { get; set; }
    public int Azure { get; set; }
    public int DevOps { get; set; }
    public int GameDev { get; set; }
    public int WebDev { get; set; }
    public int MachineLearning { get; set; }
    public int IOT { get; set; }
    public int Media { get; set; }
    public int HighScaleData { get; set; }
}

public class Engagment
{
    public string title { get; set; }
    public string description { get; set; }
    public string location { get; set; }
    public string company { get; set; }
    public string date { get; set; }
    public string[] tech { get; set; }
    public string[] resources { get; set; }
}
