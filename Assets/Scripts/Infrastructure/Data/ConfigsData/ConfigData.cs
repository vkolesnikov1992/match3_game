using System;

namespace Infrastructure.Data.ConfigsData
{
    [Serializable]
    public struct ConfigData
    {
        public string Name;
        public string SheetId;
        public SheetName[] SheetNames;
    }
}