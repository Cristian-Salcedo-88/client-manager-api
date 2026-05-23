using ClientManager.Infraestructure.Settings;

namespace ClientManager.Infraestructure.Interfaces
{
    public interface ISettingsContext
    {
        SqlServerSettings GetSqlServerSettings();
    }
}
