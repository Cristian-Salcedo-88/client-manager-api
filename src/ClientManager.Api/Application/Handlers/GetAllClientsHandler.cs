using ClientManager.Api.Application.DTO;
using ClientManager.Api.Application.Queries;
using ClientManager.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientManager.Api.Application.Handlers
{
    public class GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, List<ClientDto>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<GetAllClientsHandler> _logger;

        public GetAllClientsHandler(IClientRepository clientRepository, ILogger<GetAllClientsHandler> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<List<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("=== Se inicia consulta de todos los clientes ===");

            var clients = await _clientRepository.GetAllClientsAsync();

            return clients.Select(c => new ClientDto
            {
                Name = c.Name,
                IdentificationNumber = c.IdentificationNumber,
                Phone = c.Phone,
                Address = c.Address
            }).ToList();
        }
    }
}
