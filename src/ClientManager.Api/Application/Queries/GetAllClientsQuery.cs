using ClientManager.Api.Application.DTO;
using MediatR;
using System.Collections.Generic;

namespace ClientManager.Api.Application.Queries
{
    public class GetAllClientsQuery : IRequest<List<ClientDto>>
    {
    }
}
