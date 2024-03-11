using System;
using Infrastructure.Services.MonoProvider.Interfaces;

namespace Infrastructure.MonoBehaviour.MonoProvider
{
    public class MonoProvider : UnityEngine.MonoBehaviour, IMonoProvider
    {
        public event Action<bool> OnApplicationPauseEvent;
        private void OnApplicationPause(bool pause)
        {
            OnApplicationPauseEvent?.Invoke(pause);
        }
    }
}