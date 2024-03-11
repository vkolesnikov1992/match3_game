using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Infrastructure.MonoBehaviour.View.Core
{
    public class CubeView : UnityEngine.MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Sprite[] _idleAnimation;
        private Sprite[] _destroyAnimation;
        private CancellationTokenSource _idleCancellationTokenSource;
        private CancellationTokenSource _destroyCancellationTokenSource;

        private int _animationSpeed = 100;

        public void Initialize(Sprite[] idleAnimation, Sprite[] destroyAnimation, int animationSpeed)
        {
            _idleAnimation = idleAnimation;
            _destroyAnimation = destroyAnimation;
            
            _idleCancellationTokenSource = new CancellationTokenSource();
            _destroyCancellationTokenSource = new CancellationTokenSource();

            _animationSpeed = animationSpeed;
            
            PlayIdleAnimation(_idleCancellationTokenSource.Token).Forget();
        }
        
        private async UniTask PlayIdleAnimation(CancellationToken cancellationToken)
        {
            int index = Random.Range(0, _idleAnimation.Length);
            while (!cancellationToken.IsCancellationRequested)
            {
                _spriteRenderer.sprite = _idleAnimation[index];
                index = (index + 1) % _idleAnimation.Length;
                await UniTask.Delay(_animationSpeed, cancellationToken: cancellationToken);
            }
        }

        public async UniTask PlayDestroyAnimation()
        {
            _idleCancellationTokenSource.Cancel();

            foreach (Sprite sprite in _destroyAnimation)
            {
                _spriteRenderer.sprite = sprite;
                await UniTask.Delay(_animationSpeed, cancellationToken: _destroyCancellationTokenSource.Token);
            }

            Destroy(gameObject);
        }

        public void Destroy()
        {
            _idleCancellationTokenSource.Cancel();
            _destroyCancellationTokenSource.Cancel();
            Destroy(gameObject);
        }
    }
}