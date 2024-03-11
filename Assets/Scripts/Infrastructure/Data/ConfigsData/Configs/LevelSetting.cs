using System;
using System.Text;
using Infrastructure.Systems.Core.Components;
using Newtonsoft.Json;

namespace Infrastructure.Data.ConfigsData.Configs
{
    [Serializable]
    public struct LevelSetting
    {
        [JsonProperty("Levels")] 
        public string Level;

        public int[,] GetLevelsArray()
        {
            if (string.IsNullOrEmpty(Level))
            {
                throw new ArgumentNullException(nameof(Level));
            }

            string[] levelStrings = Level.Split(new[] { "],[" }, StringSplitOptions.None);

            int rowCount = levelStrings.Length;
            int columnCount = levelStrings[0].Split(',').Length;

            int[,] levelsArray = new int[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] values = levelStrings[i].Replace("[", "").Replace("]", "").Split(',');

                for (int j = 0; j < columnCount; j++)
                {
                    levelsArray[i, j] = int.Parse(values[j]);
                }
            }

            return levelsArray;
        }
        
        public void SaveConvertCellArrayToString(Cell[,] cellArray)
        {
            int numRows = cellArray.GetLength(0);
            int numColumns = cellArray.GetLength(1);

            StringBuilder sb = new StringBuilder();

            for (int y = numColumns - 1; y >= 0; y--)
            {
                sb.Append("[");
                for (int x = 0; x < numRows; x++)
                {
                    Cell cell = cellArray[x, y];
                    sb.Append(cell.Cube.CubeType);

                    if (x < numRows - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("]");

                if (y > 0)
                {
                    sb.Append(",");
                }
            }

            Level = sb.ToString();
        }
    }
}