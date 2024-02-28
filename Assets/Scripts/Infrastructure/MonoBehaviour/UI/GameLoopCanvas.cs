using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.MonoBehaviour.UI
{
    public class GameLoopCanvas : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _replayButton;
        
        [Inject]
        public void Construct()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(NextLevel);
            _replayButton.onClick.AddListener(ReplayLevel);
        }

        private void NextLevel()
        {
            throw new NotImplementedException();
        }

        private void ReplayLevel()
        {
            throw new NotImplementedException();
        }
    }
}