using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyStrategyFactory.Abstractions;

namespace AspNetCoreWebApiDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IStrategyFactory _strategyFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IStrategyFactory strategyFactory)
    {
        _logger = logger;
        _strategyFactory = strategyFactory;
    }

    [HttpGet]
    public WeatherForecast? Get(string checkType)
    {
        var values = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        if (!_strategyFactory.TryCreateStrategy<ICheckStrategy>(checkType, out var strategy))
        {
            return new WeatherForecast{};
        }

        foreach (var item in values)
        {
            if (strategy.Check(item))
                return item;
        }

        return new WeatherForecast{};
    }
}
