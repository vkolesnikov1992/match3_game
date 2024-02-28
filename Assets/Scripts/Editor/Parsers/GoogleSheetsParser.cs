using System.IO;
using System.Net.Http;
using UnityEngine;

namespace Editor.Parsers
{
    public class GoogleSheetsParser : IParser
    {
        private readonly string _storagePath = Application.dataPath + "/Resources/Settings";

        public async void ParseData(string id, string name)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            string apiRequest;

            try
            {
                apiRequest = $"https://opensheet.elk.sh/{id}/{name}";

                response = await client.GetAsync(apiRequest);
            }
            catch
            {
                Debug.LogError("invalid input parameters");
            }

            if (response == null)
            {
                Debug.LogError("Null response");
                return;
            }

            string directoryPath = Path.Combine(_storagePath, name);
            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            await File.WriteAllTextAsync($"{_storagePath}/{name}/{name}.json",
                response.Content.ReadAsStringAsync().Result);
            Debug.Log("Configs updated!");
        }

    }
}