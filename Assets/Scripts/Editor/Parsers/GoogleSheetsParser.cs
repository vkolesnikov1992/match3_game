using System.IO;
using System.Net.Http;
using UnityEngine;

namespace Editor.Parsers
{
    public class GoogleSheetsParser : IParser
    {
        private readonly string _storagePath = Application.dataPath + "/Resources/";

        public async void ParseData(string sheetId, string sheetName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            string apiRequest;

            try
            {
                apiRequest = $"https://opensheet.elk.sh/{sheetId}/{sheetName}";

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


            await File.WriteAllTextAsync($"{_storagePath}{sheetName}.json",
                response.Content.ReadAsStringAsync().Result);
            Debug.Log("Configs updated!");
        }

    }
}