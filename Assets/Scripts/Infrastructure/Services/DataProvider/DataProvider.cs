using Infrastructure.Data.ConfigsData.Configs;
using Infrastructure.Data.PlayerData;
using Infrastructure.Services.DataProvider.Interfaces;

namespace Infrastructure.Services.DataProvider
{
    public class DataProvider : IDataProvider
    {
        public ConfigsDataContainer ConfigsDataContainer { get; set; }
        public PlayerDataContainer PlayerDataContainer { get; set; }
    }
}