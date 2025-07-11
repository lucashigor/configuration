using AdasIt.Andor.Configurations.Application;
using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Dto;
using AdasIt.Andor.Domain.ValuesObjects;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AdasIt.Andor.Configurations.WebApi;

[ApiController]
[Route("api/configurations")]
public class ConfigurationController : ControllerBase
{
    private readonly IActorRef _configActor;

    private TimeSpan Timeout =>
#if DEBUG
        TimeSpan.FromHours(2);
#else
    TimeSpan.FromSeconds(5);
#endif

    public ConfigurationController(ActorRegistry registry)
    {
        _configActor = registry.Get<ConfigurationManagerActor>();
    }

    [HttpGet]
    public async Task<IActionResult> TaBala()
    {
        return Ok("Tá Bala!");
    }

    [HttpGet("{configId}")]
    public async Task<IActionResult> GetConfiguration([FromRoute] Guid configId)
    {
        var cts = new CancellationTokenSource(Timeout);

        var command = new GetConfiguration(configId);

        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, Timeout);

        return result.IsSuccess ? Ok(config) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConfiguration command)
    {
        var cts = new CancellationTokenSource(Timeout);

        command.CancellationToken = cts.Token;

        var (result, config) = await _configActor.Ask<(DomainResult, Configuration)>(command, Timeout);

        return result.IsSuccess ? Ok(config) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateConfiguration command)
    {
        if (id != command.Id)
            return BadRequest("Mismatched ID");

        var result = await _configActor.Ask<DomainResult>(command, Timeout);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}
