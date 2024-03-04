namespace Infrastructure.Systems.Core.Components
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public Cube Cube { get; set; }
        
        public bool IsCellLocked;
        
        
        public Cell(int x, int y, Cube cube)
        {
            X = x;
            Y = y;
            Cube = cube;
        }
    }
}