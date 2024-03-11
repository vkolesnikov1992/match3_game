using Infrastructure.MonoBehaviour.View.Core;
using UnityEngine;

namespace Infrastructure.Factory.GameFactory.Interfaces
{
    public interface IGameFactory
    {
        CubeView CreateCube(int x, int y, int type);

        BallView CreateBall(Vector3 position);
    }
}