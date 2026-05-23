using MediatR;

namespace ClientManager.Api.Application.Commands
{
    public class CreateClientCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
