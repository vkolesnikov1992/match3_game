using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.MonoBehaviour.View.Core
{
    public class BallView : UnityEngine.MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Sprite[] _sprites;
        private Tween _moveTween;
        public event Action<BallView> OnMovementComplete;
        private CancellationTokenSource _cancellationToken;

        public void Initialize(Sprite[] sprite)
        {
            _sprites = sprite;

            _cancellationToken = new CancellationTokenSource();
        }
        
        public async UniTaskVoid Move(Vector3 startPoint, Vector3 endPoint, float size, float duration, float amplitude, int delay)
        {
            await UniTask.Delay(delay, cancellationToken: _cancellationToken.Token);
            
            Vector3 scale = new Vector3(size, size, size);
            transform.localScale = scale;
            
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            
            transform.position = startPoint;

            _moveTween = transform.DOMove(endPoint, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    float t = Time.time / duration * 10;
                    float yOffset = Mathf.Sin(t) * amplitude;
                    transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, 0f);
                })
                .OnComplete(() =>
                {
                    OnMovementComplete?.Invoke(this);
                });
        }

        public void KillMovement()
        {
            _moveTween?.Kill();
            _cancellationToken.Cancel();

            _spriteRenderer.sprite = null;
            
            _cancellationToken = new CancellationTokenSource();
        }
    }
}