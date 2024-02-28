using System;

namespace Infrastructure.Data.Settings
{
    [Serializable]
    public struct SheetName
    {
        public string Name;
        public bool isSingleObject;
    }
}