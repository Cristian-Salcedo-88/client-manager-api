using ClientManager.Api.Application.Commands;
using ClientManager.Api.Application.HandlerExceptions;
using ClientManager.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ClientManager.Api.Application.Handlers
{
    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, bool>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<UpdateClientHandler> _logger;

        public UpdateClientHandler(IClientRepository clientRepository, ILogger<UpdateClientHandler> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("=== Se inicia actualización del cliente con identificación: {Identification} ===", command.IdentificationNumber);

            var existingClient = await _clientRepository.GetClientByIdentificationAsync(command.IdentificationNumber);
            if (existingClient == null)
            {
                throw new NotFoundException($"No se encontró un cliente con el número de identificación {command.IdentificationNumber}.");
            }

            existingClient.Name = command.Body.Name ?? existingClient.Name;
            existingClient.Phone = command.Body.Phone ?? existingClient.Phone;
            existingClient.Address = command.Body.Address ?? existingClient.Address;

            await _clientRepository.UpdateClientByIdentificationAsync(command.IdentificationNumber, existingClient);

            _logger.LogInformation("=== Cliente con identificación {Identification} actualizado exitosamente ===", command.IdentificationNumber);
            return true;
        }
    }
}
