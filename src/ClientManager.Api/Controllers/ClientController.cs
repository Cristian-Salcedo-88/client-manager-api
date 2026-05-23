using ClientManager.Api.Application.Commands;
using ClientManager.Api.Application.DTO;
using ClientManager.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientManager.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IMediator mediator, ILogger<ClientController> logger)
        {
            _mediator = mediator;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet("v1/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseBadRequestDto))]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients()
        {
            _logger.LogInformation("=== Se inicia consulta de todos los clientes ===");
            var response = await _mediator.Send(new GetAllClientsQuery());
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="command">Datos del cliente a crear</param>
        /// <returns></returns>
        [HttpPost("v1/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseBadRequestDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseBadRequestDto))]
        public async Task<ActionResult> CreateClient(CreateClientCommand command)
        {
            _logger.LogInformation("=== Se inicia creación del cliente con número de identificación: {Identification} ===", command.IdentificationNumber);
            return StatusCode(StatusCodes.Status201Created, await _mediator.Send(command));
        }

        /// <summary>
        /// Actualiza un cliente por su número de identificación
        /// </summary>
        /// <param name="identificationNumber">Número de identificación del cliente</param>
        /// <param name="body">Datos a actualizar</param>
        /// <returns></returns>
        [HttpPut("v1/[controller]/{identificationNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseBadRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBadRequestDto))]
        public async Task<ActionResult> UpdateClient(string identificationNumber, UpdateClientDto body)
        {
            _logger.LogInformation("=== Se inicia actualización del cliente con número de identificación: {Identification} ===", identificationNumber);
            var command = new UpdateClientCommand
            {
                IdentificationNumber = identificationNumber,
                Body = body
            };
            return StatusCode(StatusCodes.Status200OK, await _mediator.Send(command));
        }
    }
}
