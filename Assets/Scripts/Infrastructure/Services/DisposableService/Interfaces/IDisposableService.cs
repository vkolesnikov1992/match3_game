using System;

namespace Infrastructure.Services.DisposableService.Interfaces
{
    public interface IDisposableService
    {
        void Track(IDisposable disposable);
        void DisposeAll();
    }
}