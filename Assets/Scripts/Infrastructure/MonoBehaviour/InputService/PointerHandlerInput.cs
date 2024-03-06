using Infrastructure.Services.InputService.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Infrastructure.MonoBehaviour.InputService
{
    public class PointerHandlerInput : UnityEngine.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Vector2Int _pressPosition;
        private Vector2Int _releasePosition;

        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressPosition = Vector2Int.RoundToInt(eventData.pointerCurrentRaycast.worldPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _releasePosition = Vector2Int.RoundToInt(eventData.pointerCurrentRaycast.worldPosition);
            
            ProcessInput();
        }
        
        private void ProcessInput()
        {
            _inputService.ProcessInput(_pressPosition, _releasePosition);
        }
    }
}