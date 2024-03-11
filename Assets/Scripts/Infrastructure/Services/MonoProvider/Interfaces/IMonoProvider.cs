using System;

namespace Infrastructure.Services.MonoProvider.Interfaces
{
    public interface IMonoProvider
    {
        event Action<bool> OnApplicationPauseEvent;
    }
}