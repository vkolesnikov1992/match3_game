using System;
using Infrastructure.Services.InputService.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.MonoBehaviour.InputService
{
    public class InputService : UnityEngine.MonoBehaviour, IInputService, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector2Int, Vector2Int> OnSwipeDetected;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}