using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        /*
          eyJhbGciOiJSUzI1NiIsImtpZCI6ImVkNmJjOWRhMWFmMjM2ZjhlYTU2YTVkNjIyMzQwMWZmNGUwODdmMTEiLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoiVGhhbmFuYXJpbiBKYWlqYW5nIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hLS9BRmRadWNyTG1SU0JvLTdpSm1RaUpVMk45NmJHbEdSUFlMYkhkMTQ2UktxM09Zbz1zOTYtYyIsImlzcyI6Imh0dHBzOi8vc2VjdXJldG9rZW4uZ29vZ2xlLmNvbS9pc3dvcmstZDhlZDAiLCJhdWQiOiJpc3dvcmstZDhlZDAiLCJhdXRoX3RpbWUiOjE2NjI3MzA4MTgsInVzZXJfaWQiOiJBc2VqcW8zSWd2YVlKVVJVR3ZWeHJObUFDOUIzIiwic3ViIjoiQXNlanFvM0lndmFZSlVSVUd2VnhyTm1BQzlCMyIsImlhdCI6MTY2Mjc0ODk0NywiZXhwIjoxNjYyNzUyNTQ3LCJlbWFpbCI6InRoYW5hbmFyaW4uamFpQHNrLXRob25idXJpLmFjLnRoIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZ29vZ2xlLmNvbSI6WyIxMDIxNjc1MDU1OTc1ODcwNjI4MTEiXSwiZW1haWwiOlsidGhhbmFuYXJpbi5qYWlAc2stdGhvbmJ1cmkuYWMudGgiXX0sInNpZ25faW5fcHJvdmlkZXIiOiJnb29nbGUuY29tIn19.KGgHcvLUZwUCABU1s3W_a9CzkYpUIdyMyD9uGJhzD4j3AiU4b9ZDem-DLMqPJAWKBsIsdBMTYraOi5Rv9X4d_wnyEjUwi4mwNcSnj5Fdl6sRJBcIkzRrzLExxbQ08Hx1xlNS-chF2GfuqhHx2Fa0WzfSZ1UmxUd2lgV5agoGABh78zh0KsYsMcFaoFIL33dzYex-jzl4IDpPSZTWu33RalSWJTK0uH-_A77SPlw0JBqMzxWwQCopK48z51Yu3DK1CbIA1MCHduRo80s-pfW_w7wubIKvV9L9cpHd22CZBPylZuk7U09v8hOeEyZj6RTTcMmZYXNj0NP760ewwq7X3g
          
          */
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine(HttpContext.User);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}