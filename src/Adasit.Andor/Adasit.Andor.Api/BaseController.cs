using AdasIt.Andor.ApplicationDto.Results;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Adasit.Andor.Api;

public class BaseController() : ControllerBase
{
    protected IActionResult Result<T>(ApplicationResult<T> response) where T : class
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? string.Empty;

        if (response.Data is null
            && response.Warnings.Count == 0
            && response.Errors.Count == 0
            && response.Infos.Count == 0)
        {
            return NoContent();
        }

        DefaultResponse<T> responseDto = new DefaultResponse<T>()
            with
        {
            Data = response.Data,
            TraceId = traceId,
        };

        if (response.Infos.Count != 0)
        {
            responseDto.Infos.AddRange(response.Infos);
        }

        if (response.Warnings.Count != 0)
        {
            responseDto.Warnings.AddRange(response.Warnings);
        }

        if (response.Errors.Count != 0)
        {
            responseDto.Errors.AddRange(response.Errors);

            return BadRequest(responseDto);
        }

        return Ok(responseDto);
    }

    protected ApplicationResult<T> IdIsNullOrEmpty<T>(Guid? id) where T : class
    {
        var response = ApplicationResult<T>.Success();

        if (id == null || id == Guid.Empty)
        {
            var err = Errors.Validation();

            err.ChangeInnerMessage("Id cannot be null");

            response.AddError(err);
        }

        return response;
    }
}
