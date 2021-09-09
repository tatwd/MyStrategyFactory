using MyStrategyFactory.Abstractions;

namespace AspNetCoreWebApiDemo;

public interface ICheckStrategy
{
    bool Check(WeatherForecast code);
}

[Strategy("foo")]
public class FooCheckStrategy : ICheckStrategy
{
    public bool Check(WeatherForecast forecast)
    {
        return forecast.Summary == "Freezing";
    }
}

[Strategy("bar")]
public class BazCheckStrategy : ICheckStrategy
{
    public bool Check(WeatherForecast forecast)
    {
        return forecast.Summary == "Cool";
    }
}
