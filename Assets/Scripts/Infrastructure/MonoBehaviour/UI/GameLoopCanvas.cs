using System;
using Infrastructure.Systems.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.MonoBehaviour.UI
{
    public class GameLoopCanvas : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _replayButton;

        private GameplaySystem _gameplaySystem;

        public event Action OnReplayClicked;
        public event Action OnNextLevelClicked;
        

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(NextLevel);
            _replayButton.onClick.AddListener(ReplayLevel);
        }

        private void NextLevel()
        {
            OnNextLevelClicked?.Invoke();
        }

        private void ReplayLevel()
        {
            OnReplayClicked?.Invoke();
        }
    }
}