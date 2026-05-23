using System;
using System.Net;
using ClientManager.Api.Application.DTO;

namespace ClientManager.Api.Application.HandlerExceptions
{
    public class BadRequestException : Exception
    {
        public ResponseBadRequestDto ResponseBadRequestDto { get; set; }
        public HttpStatusCode Code = HttpStatusCode.BadRequest;

        public BadRequestException(string message, ResponseBadRequestDto response) : base(message)
        {
            ResponseBadRequestDto = response;
        }
    }
}
