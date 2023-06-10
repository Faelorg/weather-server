using Microsoft.AspNetCore.Mvc;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace weather_server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private Data data = new Data();

    [HttpGet("Test")]
    public async Task<ActionResult> test(string city)
    {
        var res = await new HttpClient().GetAsync("https://api.openweathermap.org/data/2.5/weather?q=" + Capitalize(
        city) + "&appid=11b33cd5e7c69c776ca7b12cde660a1f&units=metric");

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

        System.Console.WriteLine(sunrise);

        data.sunriseGMT = new DateTime().AddSeconds(sunrise).ToLocalTime();

        data.sunsetGMT = new DateTime().AddSeconds(sunset).ToLocalTime();

        return data;
    }

    string Capitalize(string s)
    {
        return s[0].ToString().ToUpper() + s.Remove(0, 1);
    }
}
