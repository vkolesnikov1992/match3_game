using Infrastructure.Data.ConfigsData.Configs;

namespace Infrastructure.Services.DataProvider.Interfaces
{
    public interface IConfigDataProvider
    {
        ConfigsDataContainer ConfigsDataContainer { get; set; }
    }
}