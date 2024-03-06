using System;
using UnityEngine;

namespace Infrastructure.Services.InputService.Interfaces
{
    public interface IInputService
    {
        event Action<Vector2Int, Vector2Int> OnSwipeDetected;

        void ProcessInput(Vector2Int pressPosition, Vector2Int releasePosition);
    }
}