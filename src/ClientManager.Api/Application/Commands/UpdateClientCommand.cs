using ClientManager.Api.Application.DTO;
using MediatR;

namespace ClientManager.Api.Application.Commands
{
    public class UpdateClientCommand : IRequest<bool>
    {
        public string IdentificationNumber { get; set; }
        public UpdateClientDto Body { get; set; }
    }
}
