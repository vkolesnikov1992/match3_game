using Cysharp.Threading.Tasks;
using Infrastructure.Factory.GameFactory.Interfaces;
using Infrastructure.MonoBehaviour.View.Core;
using Infrastructure.Services.CameraService.Interfaces;
using Infrastructure.Services.DataProvider.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.Systems.Core
{
    public class BallSystem : IInitializable
    {
        private readonly IGameFactory _gameFactory;
        private readonly ICameraService _cameraService;
        private readonly IConfigDataProvider _configDataProvider;
        private int _currentBallCount;

        private BallView[] _ballViews;

        public BallSystem(IGameFactory gameFactory, ICameraService cameraService, IConfigDataProvider configDataProvider)
        {
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _configDataProvider = configDataProvider;
        }

        public async void Initialize()
        {
            _ballViews = new BallView[_configDataProvider.ConfigsDataContainer.GameSettings.MaxBallsCountInGame];
            
            for (int i = 0; i < _ballViews.Length; i++)
            {
                _ballViews[i] = _gameFactory.CreateBall(Vector3.zero);
                
                int delay = Random.Range(500, 10000);
                await UniTask.Delay(delay);
                
                BallMove(_ballViews[i]);

                _ballViews[i].OnMovementComplete += BallMove;
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

            ballView.Move(startPosition, endPosition, size, duration, amplitude);
        }
    }
}