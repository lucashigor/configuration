using System.Net;

namespace AdasIt.Andor.ApplicationDto.Results;

public interface IApiException
{
    List<ErrorModel> Errors { get; }
    HttpStatusCode Status { get; }
}
