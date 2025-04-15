using System.Net;
using System.Text.Json;

namespace IMS.API.Middleware;

/// <summary>
/// 
/// </summary>
public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="next"></param>
	/// <param name="logger"></param>
	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		_logger.LogError(exception, "An unhandled exception occurred");

		var statusCode = exception switch
		{
			UnauthorizedAccessException => HttpStatusCode.Unauthorized,
			InvalidOperationException => HttpStatusCode.BadRequest,
			ArgumentException => HttpStatusCode.BadRequest,
			_ => HttpStatusCode.InternalServerError
		};

		var response = new
		{
			StatusCode = (int)statusCode,
			Message = exception.Message
		};

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		var json = JsonSerializer.Serialize(response, options);
		await context.Response.WriteAsync(json);
	}
}
