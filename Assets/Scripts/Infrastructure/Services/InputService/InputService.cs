using System;
using Infrastructure.Services.InputService.Interfaces;
using UnityEngine;

namespace Infrastructure.Services.InputService
{
    public class InputService : IInputService
    {
        public event Action<Vector2Int, Vector2Int> OnSwipeDetected;
        
        public void ProcessInput(Vector2Int pressPosition, Vector2Int releasePosition)
        {
            DetectSwipe(pressPosition, releasePosition);
        }

        private void DetectSwipe(Vector2Int pressPosition, Vector2Int releasePosition)
        {
            Vector2Int swipeVector = releasePosition - pressPosition;

            if (swipeVector.magnitude == 0)
            {
                return;
            }

            bool isHorizontalSwipe = Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y);

            Vector2Int swipeDirection = isHorizontalSwipe ?
                (swipeVector.x > 0 ? Vector2Int.right : Vector2Int.left) :
                (swipeVector.y > 0 ? Vector2Int.up : Vector2Int.down);

            OnSwipeDetected?.Invoke(pressPosition, swipeDirection);
        }
    }
}