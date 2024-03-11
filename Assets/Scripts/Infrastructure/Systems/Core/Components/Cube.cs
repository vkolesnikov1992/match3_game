using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.MonoBehaviour.View.Core;
using UnityEngine;

namespace Infrastructure.Systems.Core.Components
{
    public class Cube
    {
        public int CubeType { get; set; }
        public CubeView CubeView { get; set; }

        public bool IsHasView => CubeView != null;

        public bool IsMatched;

        private readonly int _dropSpeed;
        private readonly int _swapSpeed;


        public Cube(int cubeType, CubeView cubeView, int dropSpeed, int swapSpeed)
        {
            CubeType = cubeType;
            CubeView = cubeView;
            _dropSpeed = dropSpeed;
            _swapSpeed = swapSpeed;
        }

        public async UniTask ReplaceCell(Cell cell)
        {
            if (CubeView != null)
            {
                Vector2 position = new Vector2(cell.X, cell.Y);
                CubeView.transform.DOMove(position, _swapSpeed / 1000f);
            }

            await UniTask.Delay(_swapSpeed); 
            cell.Cube = this;
        }
        
        public async UniTask Drop(Vector2 position, int delay)
        {
            CubeView.transform.DOMove(position, _dropSpeed / 1000f * delay);
            await UniTask.Delay(_dropSpeed * delay);
        }
    }
}