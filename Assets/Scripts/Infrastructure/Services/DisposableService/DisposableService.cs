using System;
using System.Collections.Generic;
using Infrastructure.Services.DisposableService.Interfaces;

namespace Infrastructure.Services.DisposableService
{
    public class DisposableService : IDisposableService
    {
        private readonly List<IDisposable> _disposablesContainer = new();
        
        public void Track(IDisposable disposable)
        {
            _disposablesContainer.Add(disposable);
        }
        
        public void DisposeAll()
        {
            foreach (var customDisposable in _disposablesContainer)
            {
                customDisposable.Dispose();
            }
            
            _disposablesContainer.Clear();
        }
    }
}