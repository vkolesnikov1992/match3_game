using System;
using Infrastructure.Factory.GameFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factory.GameFactory
{
    public class GameFactory : IGameFactory
    {
        [Inject]
        public void LoadTextures()
        {
            throw new NotImplementedException();
        }
        
        public void CreateCube(int x, int y, int type)
        {
            throw new NotImplementedException();
        }

        public void CreateBall(int index)
        {
            
        }
    }
}