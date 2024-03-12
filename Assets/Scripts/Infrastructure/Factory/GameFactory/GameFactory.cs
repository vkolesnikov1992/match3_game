using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory.GameFactory.Interfaces;
using Infrastructure.MonoBehaviour.View.Core;
using Infrastructure.Services.DataProvider.Interfaces;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory.GameFactory
{
    public class GameFactory : IGameFactory
    {
        private readonly IConfigDataProvider _configDataProvider;
        private readonly Dictionary<string, Sprite[]> _animationContainer = new();

        private Sprite[] _ballsSprites;
            
        private Transform _cubeViewHolder;
        private Transform _ballViewHolder;
        
        private CubeView _cubeViewPrefab;
        private BallView _ballViewPrefab;

        public GameFactory(IConfigDataProvider configDataProvider)
        {
            _configDataProvider = configDataProvider;
        }

        public CubeView CreateCube(int x, int y, int type)
        {
            string[] atlases = { $"Atlases/{type}_idle", $"Atlases/{type}_destroy" };

            LoadAtlases(atlases);

            if (_cubeViewPrefab == null)
            {
                _cubeViewPrefab = LoadPrefab<CubeView>();
            }

            if (_cubeViewHolder == null)
            {
                _cubeViewHolder = new GameObject("CubeHolder").transform;
            }

            Vector3 position = new Vector3(x, y, 0);
            CubeView cubeView = Object.Instantiate(_cubeViewPrefab, position, Quaternion.identity, _cubeViewHolder);
            
            int animationSpeed = _configDataProvider.ConfigsDataContainer.GameSettings.CubeAnimationSpeed;
            
            cubeView.Initialize(_animationContainer[atlases[0]], _animationContainer[atlases[1]], animationSpeed);
            
            return cubeView;
        }

        public BallView CreateBall(Vector3 position)
        {
            if (_ballViewPrefab == null)
            {
                _ballViewPrefab = LoadPrefab<BallView>();
            }
            
            if (_ballViewHolder == null)
            {
                _ballViewHolder = new GameObject("BallHolder").transform;
            }

            if (_ballsSprites == null)
            {
                SpriteAtlas atlas = Resources.Load<SpriteAtlas>($"Atlases/balloons");
                Sprite[] sprites = new Sprite[atlas.spriteCount];
                atlas.GetSprites(sprites);
                _ballsSprites = sprites;
            }
            
            BallView ballView = Object.Instantiate(_ballViewPrefab, position, Quaternion.identity, _ballViewHolder);
            ballView.Initialize(_ballsSprites);
            
            return ballView;
        }

        private bool IsAtlasExist(Dictionary<string, Sprite[]> container, string path)
        {
            return container.ContainsKey(path);
        }

        private void LoadAtlas(Dictionary<string, Sprite[]> container, string path)
        {
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>(path);
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            
            Array.Sort(sprites, CompareSpritesByName);
            
            container.Add(path, sprites);
        }
        
        private int CompareSpritesByName(Sprite a, Sprite b)
        {
            string nameA = a.name;
            string nameB = b.name;

            nameA = nameA.Replace("(Clone)", "");
            nameB = nameB.Replace("(Clone)", "");

            string[] splitNameA = nameA.Split('_');
            string[] splitNameB = nameB.Split('_');

            if (splitNameA.Length > 1 && splitNameB.Length > 1)
            {
                string lastPartA = splitNameA.Last();
                string lastPartB = splitNameB.Last();

                if (int.TryParse(lastPartA, out int numberA) && int.TryParse(lastPartB, out int numberB))
                {
                    return numberA.CompareTo(numberB);
                }
            }

            return 0;
        }
        
        private T LoadPrefab<T>() where T : UnityEngine.MonoBehaviour
        {
            string path = $"Prefabs/{typeof(T).Name}";
            T prefab = Resources.Load<T>(path);
            return prefab;
        }
        
        private void LoadAtlases(string[] atlasNames)
        {
            foreach (string atlasName in atlasNames)
            {
                if (!IsAtlasExist(_animationContainer, atlasName))
                {
                    LoadAtlas(_animationContainer, atlasName);
                }
            }
        } 
    }
}