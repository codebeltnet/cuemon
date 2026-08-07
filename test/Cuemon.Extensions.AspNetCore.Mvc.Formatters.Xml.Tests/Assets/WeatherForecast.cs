using System;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Assets;
public class WeatherForecast
{
    public WeatherForecast()
    {
        Date = DateTime.UtcNow;
        TemperatureC = Generate.RandomNumber(-20, 55);
        Summary = "Scorching";
    }

    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }
}
