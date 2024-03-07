using Infrastructure.Data.PlayerData;

namespace Infrastructure.Services.DataProvider.Interfaces
{
    public interface IPlayerDataProvider
    {
        PlayerDataContainer PlayerDataContainer { get; set; }
    }
}