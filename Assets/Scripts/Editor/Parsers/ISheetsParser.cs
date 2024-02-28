namespace Editor.Parsers
{
    public interface ISheetsParser : IParser
    {
        new void ParseData(string generalName, string id, string name);
    }
}