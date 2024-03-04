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


        public Cube(int cubeType, CubeView cubeView)
        {
            CubeType = cubeType;
            CubeView = cubeView;
        }

        public async UniTask ReplaceCell(Cell cell)
        {
            if (CubeView != null)
            {
                Vector2 position = new Vector2(cell.X, cell.Y);
                CubeView.transform.DOMove(position, 0.5f);
            }

            await UniTask.Delay(500); 
            cell.Cube = this;
        }
        
        public async UniTask Drop(Vector2 position, int delay)
        {
            CubeView.transform.DOMove(position, 0.5f * delay);
            await UniTask.Delay(500 * delay);
        }
    }
}