using Microsoft.AspNetCore.Mvc;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace weather_server.Controllers;

[ApiController]
[Route("/")]
public class WeatherForecastController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public WeatherForecastController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private Data data = new Data();

    [HttpGet("get")]
    public async Task<ActionResult> GetWeather(string city)
    {
        var res = await new HttpClient().GetAsync("https://api.openweathermap.org/data/2.5/weather?q=" + Capitalize(
        city) + "&appid=" + Environment.GetEnvironmentVariable("API_KEY") + "&units=metric");
        var rs = await res.Content.ReadAsStringAsync();
        return Ok(Prettify(rs));
    }

    object Prettify(string str)
    {
        var parsedData = JObject.Parse(str);

        var temp = parsedData.SelectToken("main.temp")!.ToString();

        data.celcium = temp;

        var place = parsedData.SelectToken("name")!.ToString();

        data.place = place;

        var description = parsedData.SelectToken("weather[0].description")!.ToString();

        data.description = description;

        var icon = parsedData.SelectToken("weather[0].icon")!.ToString();

        data.icon = icon;

        data.iconUrl = "http://openweathermap.org/img/wn/" + data.icon + "@2x.png";

        var sunrise = double.Parse(parsedData.SelectToken("sys.sunrise")!.ToString());

        var sunset = double.Parse(parsedData.SelectToken("sys.sunset")!.ToString());

        data.sunriseGMT = new DateTime().AddSeconds(sunrise).ToLocalTime();

        data.sunsetGMT = new DateTime().AddSeconds(sunset).ToLocalTime();

        return data;
    }

    string Capitalize(string s)
    {
        return s[0].ToString().ToUpper() + s.Remove(0, 1);
    }
}
