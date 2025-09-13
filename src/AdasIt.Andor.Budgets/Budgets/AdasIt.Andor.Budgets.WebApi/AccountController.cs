using Adasit.Andor.Api;
using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Budget.Application.Interfaces;
using AdasIt.Andor.Budgets.ApplicationDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AdasIt.Andor.Budgets.WebApi;

[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AccountController(
    IAccountCommandsService configurationCommands,
    IAccountQueriesService configurationQueries)
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
    [ProducesResponseType(typeof(DefaultResponse<AccountOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateAccount input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return UnprocessableEntity();
        }

        var output = await configurationCommands.CreateAccountAsync(input, cancellationToken);

        return Result(output);
    }
}
