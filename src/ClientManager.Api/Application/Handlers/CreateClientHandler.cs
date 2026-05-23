using ClientManager.Api.Application.Commands;
using ClientManager.Domain.AggregateModel;
using ClientManager.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ClientManager.Api.Application.Handlers
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, bool>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<CreateClientHandler> _logger;

        public CreateClientHandler(IClientRepository clientRepository, ILogger<CreateClientHandler> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("=== Se inicia creación del cliente con identificación: {Identification} ===", command.IdentificationNumber);

            var existingClient = await _clientRepository.GetClientByIdentificationAsync(command.IdentificationNumber);
            if (existingClient != null)
            {
                throw new Domain.Exceptions.DomainException($"Ya existe un cliente con el número de identificación {command.IdentificationNumber}.");
            }

            var client = new Client(command.Name, command.IdentificationNumber, command.Phone, command.Address);
            await _clientRepository.CreateClientAsync(client);

            _logger.LogInformation("=== Cliente con identificación {Identification} creado exitosamente ===", command.IdentificationNumber);
            return true;
        }
    }
}
