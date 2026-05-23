using ClientManager.Domain.AggregateModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientManager.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllClientsAsync();
        Task CreateClientAsync(Client client);
        Task<bool> UpdateClientByIdentificationAsync(string identificationNumber, Client client);
        Task<Client> GetClientByIdentificationAsync(string identificationNumber);
    }
}
