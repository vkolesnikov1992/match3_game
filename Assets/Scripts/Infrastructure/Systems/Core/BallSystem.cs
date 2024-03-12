using System;
using Infrastructure.Factory.GameFactory.Interfaces;
using Infrastructure.MonoBehaviour.View.Core;
using Infrastructure.Services.CameraService.Interfaces;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.DisposableService.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Systems.Core
{
    public class BallSystem : IInitializable, IDisposable
    {
        private readonly IGameFactory _gameFactory;
        private readonly ICameraService _cameraService;
        private readonly IConfigDataProvider _configDataProvider;
        private readonly IDisposableService _disposableService;

        private BallView[] _ballViews;

        public BallSystem(IGameFactory gameFactory, ICameraService cameraService, 
            IConfigDataProvider configDataProvider, IDisposableService disposableService)
        {
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _configDataProvider = configDataProvider;
            _disposableService = disposableService;
        }

        public void Initialize()
        {
            _disposableService.Track(this);
            _ballViews = new BallView[_configDataProvider.ConfigsDataContainer.GameSettings.MaxBallsCountInGame];
            
            for (int i = 0; i < _ballViews.Length; i++)
            {
                _ballViews[i] = _gameFactory.CreateBall(Vector3.zero);
            }
            
            for (int i = 0; i < _ballViews.Length; i++)
            {
                BallMove(_ballViews[i]);

                _ballViews[i].OnMovementComplete += BallMove;
            }
        }
        
        public void Dispose()
        {
            foreach (BallView ballView in _ballViews)
            {
                ballView.OnMovementComplete -= BallMove;
            }
        }
        
        public void RestartBallMovement()
        {
            for (var i = 0; i < _ballViews.Length; i++)
            {
                BallView ballView = _ballViews[i];
                ballView.KillMovement();
                BallMove(ballView);
            }
        }

        private void BallMove(BallView ballView)
        {
            Vector3[] positions = _cameraService.GenerateRandomPointsOutsideCamera();

            Vector3 startPosition = positions[0];
            Vector3 endPosition = positions[1];

            float size = Random.Range(0.6f, 0.8f);
            float duration = Random.Range(4f, 20f);
            float amplitude = Random.Range(0.5f, 1f);
            int delay = Random.Range(3000, 15000);
            
            ballView.Move(startPosition, endPosition, size, duration, amplitude, delay).Forget();
        }
    }
}