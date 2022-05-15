using CustomException.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace CustomException.Middleware
{
    public static class ExceptionMiddleware
    {
        public static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            switch (exception)
            {
                case SecurityTokenExpiredException:
                    await SetException(httpContext, StatusCodes.Status401Unauthorized, "token expired");
                    break;
                case SecurityTokenValidationException:
                    await SetException(httpContext, StatusCodes.Status401Unauthorized, "token is not valid");
                    break;
                case AccessViolationException:
                    await SetException(httpContext, StatusCodes.Status401Unauthorized, exception.Message);
                    break;
                case BadHttpRequestException:
                    await SetException(httpContext, StatusCodes.Status400BadRequest, exception.Message);
                    break;
                case KeyNotFoundException:
                    await SetException(httpContext, StatusCodes.Status404NotFound, exception.Message);
                    break;
                case DuplicateNameException:
                    await SetException(httpContext, StatusCodes.Status409Conflict, exception.Message);
                    break;
                case SqlException:
                    await SetException(httpContext, StatusCodes.Status500InternalServerError, "Database Error");
                    break;
                case NotFoundException:
                    await SetException(httpContext, StatusCodes.Status404NotFound, exception.Message);
                    break;
                case NotAcceptableException:
                    await SetException(httpContext, StatusCodes.Status406NotAcceptable, exception.Message);
                    break;
                case ForbiddenException:
                    await SetException(httpContext, StatusCodes.Status403Forbidden, exception.Message);
                    break;
                case MethodNotAllowedException:
                    await SetException(httpContext, StatusCodes.Status405MethodNotAllowed, exception.Message);
                    break;
                case ConflictException:
                    await SetException(httpContext, StatusCodes.Status409Conflict, exception.Message);
                    break;
                default:
                    await SetException(httpContext, StatusCodes.Status500InternalServerError, exception.Message);
                    break;
            }
        }
        private static async Task SetException(HttpContext context, int code, string message)
        {
            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = code,
                Message = message
            }.ToString());
        }
    }
}
