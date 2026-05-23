using ClientManager.Domain.AggregateModel;
using ClientManager.Domain.Interfaces;
using ClientManager.Infraestructure.Exceptions;
using ClientManager.Infraestructure.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClientManager.Infraestructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(ISettingsContext settingsContext, ILogger<ClientRepository> logger)
        {
            _connectionString = settingsContext.GetSqlServerSettings().ConnectionStrings;
            _logger = logger;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<Client>(
                    "SELECT Name, IdentificationNumber, Phone, Address FROM dbo.Clientes"
                );
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los clientes");
                throw new InfraestructureException("Error al obtener los clientes de la base de datos.", ex);
            }
        }

        public async Task<Client> GetClientByIdentificationAsync(string identificationNumber)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<Client>(
                    "SELECT Name, IdentificationNumber, Phone, Address FROM dbo.Clientes WHERE IdentificationNumber = @IdentificationNumber",
                    new { IdentificationNumber = identificationNumber }
                );
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente por identificación {Identification}", identificationNumber);
                throw new InfraestructureException($"Error al obtener el cliente con identificación {identificationNumber}.", ex);
            }
        }

        public async Task CreateClientAsync(Client client)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(
                    "INSERT INTO dbo.Clientes (Name, IdentificationNumber, Phone, Address) VALUES (@Name, @IdentificationNumber, @Phone, @Address)",
                    new { client.Name, client.IdentificationNumber, client.Phone, client.Address }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el cliente con identificación {Identification}", client.IdentificationNumber);
                throw new InfraestructureException($"Error al crear el cliente con identificación {client.IdentificationNumber}.", ex);
            }
        }

        public async Task<bool> UpdateClientByIdentificationAsync(string identificationNumber, Client client)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var rowsAffected = await connection.ExecuteAsync(
                    "UPDATE dbo.Clientes SET Name = @Name, Phone = @Phone, Address = @Address WHERE IdentificationNumber = @IdentificationNumber",
                    new { client.Name, client.Phone, client.Address, IdentificationNumber = identificationNumber }
                );
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con identificación {Identification}", identificationNumber);
                throw new InfraestructureException($"Error al actualizar el cliente con identificación {identificationNumber}.", ex);
            }
        }
    }
}
