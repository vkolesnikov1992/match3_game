using Infrastructure.MonoBehaviour.View.Core;

namespace Infrastructure.Factory.GameFactory.Interfaces
{
    public interface IGameFactory
    {
        CubeView CreateCube(int x, int y, int type);
        
    }
}