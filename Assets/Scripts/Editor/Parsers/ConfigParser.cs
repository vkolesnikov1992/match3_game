using Infrastructure.Data.ConfigsData;
using Infrastructure.Data.Resources.Settings;
using UnityEditor;
using UnityEngine;

namespace Editor.Parsers
{
    public static class ConfigParser
    {
        private const string ConfigPath = "Settings/ConfigDataPaths";

        [MenuItem("Tools/Parse Config Data")]
        public static void ParseConfigData()
        {
            ConfigData[] configsData = Resources.Load<ConfigDataPaths>(ConfigPath).ConfigsData;

            IParser parser = new GoogleSheetsParser();

            foreach (ConfigData config in configsData)
            {
                foreach (SheetName sheetName in config.SheetNames)
                {
                    parser.ParseData(config.SheetId, sheetName.Name);
                }

            }
        }
    }
}