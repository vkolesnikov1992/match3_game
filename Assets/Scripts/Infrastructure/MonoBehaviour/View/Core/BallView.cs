using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.MonoBehaviour.View.Core
{
    public class BallView : UnityEngine.MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Sprite[] _sprites;
        
        public event Action<BallView> OnMovementComplete;

        public void SetSprites(Sprite[] sprite)
        {
            _sprites = sprite;
        }
        
        public void Move(Vector3 startPoint, Vector3 endPoint, float size, float duration, float amplitude)
        {
            Vector3 scale = new Vector3(size, size, size);
            transform.localScale = scale;
            
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            
            transform.position = startPoint;

            transform.DOMove(endPoint, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    float t = Time.time / duration * 10;
                    float yOffset = Mathf.Sin(t) * amplitude;
                    transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, 0f);
                })
                .OnComplete(() =>
                {
                    float delay = Random.Range(1f, 10f);
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        OnMovementComplete?.Invoke(this);
                    });
                });
        }
    }
}