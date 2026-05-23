using ClientManager.Infraestructure.Interfaces;
using ClientManager.Infraestructure.Settings;
using Microsoft.Extensions.Configuration;

namespace ClientManager.Infraestructure.Context
{
    public class SettingsContext : ISettingsContext
    {
        private readonly IConfiguration _configuration;

        public SettingsContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlServerSettings GetSqlServerSettings() => _configuration.GetSection("SqlServerSettings").Get<SqlServerSettings>();
    }
}
