using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace Hacker.API.Infrastructure;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid();

        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await SetErrorResponse(context, HttpStatusCode.BadRequest, ex.Message, correlationId);
        }
        catch (Exception ex)
        {
            
            await SetErrorResponse(context, HttpStatusCode.InternalServerError, ex.Message, correlationId);
        }
    }

    private static async Task SetErrorResponse(HttpContext context, HttpStatusCode statusCode, string message, Guid correlationId)
    {
        var response = context.Response;

        response.Clear();
        response.StatusCode = (int)statusCode;
        response.ContentType = MediaTypeNames.Application.Json;

        var error = new
        {
            StatusCode = statusCode,
            Message = message,
            CorrelationId = correlationId
        };

        await response.WriteAsJsonAsync(error);
    }
}
