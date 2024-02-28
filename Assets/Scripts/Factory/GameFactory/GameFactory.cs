using System;
using Factory.GameFactory.Interfaces;
using Zenject;

namespace Factory.GameFactory
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
    }
}