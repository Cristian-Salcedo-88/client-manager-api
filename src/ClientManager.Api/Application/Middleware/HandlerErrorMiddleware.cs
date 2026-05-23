using ClientManager.Api.Application.DTO;
using ClientManager.Api.Application.HandlerExceptions;
using ClientManager.Domain.Exceptions;
using ClientManager.Infraestructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ClientManager.Api.Application
{
    public class HandlerErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlerErrorMiddleware> _logger;

        public HandlerErrorMiddleware(RequestDelegate next, ILogger<HandlerErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandlerExceptionAsync(context, ex);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            object errors = null;

            switch (ex)
            {
                case BadRequestException me:
                    _logger.LogError(ex, "Handler Error - Bad Request");
                    errors = me.ResponseBadRequestDto;
                    context.Response.StatusCode = (int)me.Code;
                    break;
                case InfraestructureException ie:
                    _logger.LogError(ex, "Exception Infra Server");
                    string iErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    errors = BuildErrorResponse(iErrorMessage, HttpStatusCode.InternalServerError);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case NotFoundException me:
                    _logger.LogError(ex, "Handler Error - Not Found");
                    errors = BuildErrorResponse(me.Message, HttpStatusCode.NotFound);
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case DomainException me:
                    _logger.LogError(ex, "Handler Error - Domain");
                    errors = BuildErrorResponse(me.Message, HttpStatusCode.Conflict);
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                default:
                    _logger.LogError(ex, "Error Server");
                    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    errors = BuildErrorResponse(errorMessage, HttpStatusCode.InternalServerError);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (errors != null)
            {
                var result = JsonConvert.SerializeObject(errors);
                await context.Response.WriteAsync(result);
            }
        }

        private static ResponseBadRequestDto BuildErrorResponse(string message, HttpStatusCode statusCode)
        {
            return new ResponseBadRequestDto
            {
                Title = message,
                Status = (int)statusCode,
                Errors = new System.Collections.Generic.Dictionary<string, string[]>
                {
                    { "Error", new[] { message } }
                }
            };
        }
    }
}
