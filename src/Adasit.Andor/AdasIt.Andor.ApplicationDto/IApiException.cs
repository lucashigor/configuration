using System.Net;

namespace AdasIt.Andor.ApplicationDto;

public interface IApiException
{
    List<ErrorModel> Errors { get; }
    HttpStatusCode Status { get; }
}
