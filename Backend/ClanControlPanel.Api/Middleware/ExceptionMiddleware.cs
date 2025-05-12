using System.Text.Json;
using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO.Response;

namespace ClanControlPanel.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            
            var response = context.Response;
            var error = new ErrorResponse();

            switch (ex)
            {
                case Exception notFoundEx when notFoundEx.GetType().IsGenericType 
                                               && notFoundEx.GetType().GetGenericTypeDefinition() == typeof(EntityNotFoundException<>):
                    response.StatusCode = StatusCodes.Status404NotFound;
                    error.Message = notFoundEx.Message;
                    break;
                
                case Exception entityIsExistsEx when entityIsExistsEx.GetType().IsGenericType 
                                                     && entityIsExistsEx.GetType().GetGenericTypeDefinition() == typeof(EntityIsExists<>):
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    error.Message = entityIsExistsEx.Message;
                    break;

                case Exception playerIsNotInSquadEx when playerIsNotInSquadEx.GetType().IsGenericType 
                                                         && playerIsNotInSquadEx.GetType().GetGenericTypeDefinition() == typeof(PlayerIsNotInSquad<>):
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    error.Message = playerIsNotInSquadEx.Message;
                    break;

                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    error.Message = "Server error\n";
                    error.Message += ex.Message;
                    break;
            }
            error.StatusCode = response.StatusCode;
            
            var json = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(json);
        }
    }
}