using System;
using Infrastructure.Services.InputService.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.MonoBehaviour.InputService
{
    public class InputService : UnityEngine.MonoBehaviour, IInputService, IPointerDownHandler, IPointerUpHandler
    {
        private Vector2Int _pressPosition;
        private Vector2Int _releasePosition;
        public event Action<Vector2Int, Vector2Int> OnSwipeDetected;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _pressPosition = Vector2Int.RoundToInt(eventData.pointerCurrentRaycast.worldPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _releasePosition = Vector2Int.RoundToInt(eventData.pointerCurrentRaycast.worldPosition);
            
            DetectSwipe();
        }
        
        private void DetectSwipe()
        {
            Vector2Int swipeVector = _releasePosition - _pressPosition;

            if (swipeVector.magnitude == 0)
            {
                return;
            }
            
            bool isHorizontalSwipe = Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y);

            Vector2Int swipeDirection = isHorizontalSwipe ?
                (swipeVector.x > 0 ? Vector2Int.right : Vector2Int.left) :
                (swipeVector.y > 0 ? Vector2Int.up : Vector2Int.down);

            OnSwipeDetected?.Invoke(_pressPosition, swipeDirection);
        }
    }
}