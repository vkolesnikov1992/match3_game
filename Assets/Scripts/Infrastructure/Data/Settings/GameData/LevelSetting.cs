using System;
using Newtonsoft.Json;

namespace Infrastructure.Data.Settings.GameData
{
    [Serializable]
    public struct LevelSetting
    {
        [JsonProperty("Levels")] 
        private string _level;

        public int[,] GetLevelsArray()
        {
            if (string.IsNullOrEmpty(_level))
            {
                throw new ArgumentNullException(nameof(_level));
            }

            string[] levelStrings = _level.Split(new[] { "],[" }, StringSplitOptions.None);

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
    }
}