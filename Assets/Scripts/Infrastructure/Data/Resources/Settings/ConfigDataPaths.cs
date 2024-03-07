using Infrastructure.Data.ConfigsData;
using UnityEngine;

namespace Infrastructure.Data.Resources.Settings
{
    [CreateAssetMenu(fileName = "ConfigDataPaths", menuName = "ScriptableObjects/ConfigDataPaths", order = 1)]
    public class ConfigDataPaths : ScriptableObject
    {
        public ConfigData[] ConfigsData;
    }
}