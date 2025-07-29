using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdasIt.Andor.Configurations.WebApi;

[ApiController]
[Route("api/configurations")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationCommandsService _configurationCommands;
    private readonly IConfigurationQueriesService _configurationQueries;

    public ConfigurationController(IConfigurationCommandsService configurationCommands, IConfigurationQueriesService configurationQueries)
    {
        _configurationCommands = configurationCommands;
        _configurationQueries = configurationQueries;
    }

    [HttpGet]
    public IActionResult TaBala()
    {
        return Ok("My First Heading!");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetConfigurationAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var config = await _configurationQueries.GetByIdAsync(id, cancellationToken);

        return Ok(config);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConfiguration command, CancellationToken cancellationToken)
    {
        await _configurationCommands.CreateConfigurationAsync(command, cancellationToken);

        return Accepted();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateConfiguration command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Configuration ID in the route does not match the ID in the command.");
        }

        await _configurationCommands.UpdateConfigurationAsync(command, cancellationToken);

        return Accepted();
    }
}
