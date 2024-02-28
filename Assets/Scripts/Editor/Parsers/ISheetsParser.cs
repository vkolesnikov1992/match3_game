namespace Editor.Parsers
{
    public interface ISheetsParser : IParser
    {
        void ParseData(string sheetId, string sheetName);
    }
}