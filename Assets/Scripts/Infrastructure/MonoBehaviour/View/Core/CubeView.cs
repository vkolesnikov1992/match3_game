﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.MonoBehaviour.View.Core
{
    public class CubeView : UnityEngine.MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Sprite[] _idleAnimation;
        private Sprite[] _destroyAnimation;
        private CancellationTokenSource _cancellationTokenSource;

        private int _animationSpeed;

        [Inject]
        public void Construct()
        {
            throw new NotImplementedException();
            _animationSpeed = 0;
        }

        private void ctor(Sprite[] idleAnimation, Sprite[] destroyAnimation)
        {
            throw new NotImplementedException();
            _cancellationTokenSource = new CancellationTokenSource();
            PlayIdleAnimation(_cancellationTokenSource.Token).Forget();
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
            _cancellationTokenSource.Cancel();

            foreach (Sprite sprite in _destroyAnimation)
            {
                _spriteRenderer.sprite = sprite;
                await UniTask.Delay(_animationSpeed);
            }

            Destroy(gameObject);
        }
    }
}