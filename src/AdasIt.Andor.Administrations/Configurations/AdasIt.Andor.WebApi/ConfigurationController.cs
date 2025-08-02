using Adasit.Andor.Api;
using AdasIt.Andor.ApplicationDto;
using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.DomainQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AdasIt.Andor.Configurations.WebApi;

[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ConfigurationController(
    IConfigurationCommandsService configurationCommands,
    IConfigurationQueriesService configurationQueries)
    : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetConfigurationAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var config = await configurationQueries.GetByIdAsync(id, cancellationToken);

        return Ok(config);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(DefaultResponse<ConfigurationOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ConfigurationInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return UnprocessableEntity();
        }

        var output = await configurationCommands.CreateConfigurationAsync(input, cancellationToken);

        return Result(output);
    }

    [HttpPut("{id:guid}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(DefaultResponse<ConfigurationOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] ConfigurationInput input,
        CancellationToken cancellationToken)
    {

        if (input == null)
        {
            return UnprocessableEntity();
        }

        var output = IdIsNullOrEmpty<ConfigurationOutput>(id);

        if (output.IsFailure)
        {
            return Result(output);
        }

        output =
            await configurationCommands.UpdateConfigurationAsync(id, input, cancellationToken);

        return Result(output);
    }
}
