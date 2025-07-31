using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.ApplicationDto;
using Microsoft.AspNetCore.Mvc;

namespace AdasIt.Andor.Configurations.WebApi;

[ApiController]
[Route("api/configurations")]
public class ConfigurationController(
    IConfigurationCommandsService configurationCommands,
    IConfigurationQueriesService configurationQueries)
    : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetConfigurationAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var config = await configurationQueries.GetByIdAsync(id, cancellationToken);

        return Ok(config);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ConfigurationInput input, CancellationToken cancellationToken)
    {
        var (_, config) = 
            await configurationCommands.CreateConfigurationAsync(input, cancellationToken);

        return Ok(config);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, 
        [FromBody] ConfigurationInput input, 
        CancellationToken cancellationToken)
    {
        var (_, config) = 
            await configurationCommands.UpdateConfigurationAsync(id, input, cancellationToken);

        return Ok(config);
    }
}
