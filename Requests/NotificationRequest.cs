namespace LojaVirtual.Requests;

public class NotificationRequest
{
    public int id { get; set; }
    public bool live_mode { get; set; }
    public string type { get; set; }
    public DateTime date_created { get; set; }
    public int application_id { get; set; }
    public int user_id { get; set; }
    public int version { get; set; }
    public string api_version { get; set; }
    public string action { get; set; }
    public Data data { get; set; }
}


public class Data
{
    public string id { get; set; }
}

