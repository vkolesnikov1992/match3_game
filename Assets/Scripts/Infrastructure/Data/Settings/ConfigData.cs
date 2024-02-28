using System;

namespace Infrastructure.Data.Settings
{
    [Serializable]
    public struct ConfigData
    {
        public string Name;
        public string SheetId;
        public SheetName[] SheetNames;
    }
}