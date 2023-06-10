namespace weather_server;

public class Data
{
    public string celcium { get; set; } = string.Empty;

    public string place { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public string icon { get; set; } = string.Empty;

    public DateTime sunriseGMT { get; set; }

    public DateTime sunsetGMT { get; set; }

    public string iconUrl { get; set; } = string.Empty;
}

