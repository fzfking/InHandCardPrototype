using System.IO;
using Codebase.DataTransferObjects;
using Codebase.Infrastructure.Services.Loaders;
using Newtonsoft.Json;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Saver
{
    public class CardSetupsSaver
    {
        private static string CardSetupsPath => CardSetupsLoader.CardSetupsPath;

        public CardSetupsSaver()
        {
        }

        public void SaveSetups(CardSetup[] setups)
        {
            var serialized = SerializeCardSetups(setups);
            SaveJson(serialized);
        }

        private void SaveJson(string json)
        {
            using (StreamWriter streamWriter = new StreamWriter(CardSetupsPath))
            {
                streamWriter.Write(json);
            }
        }

        private string SerializeCardSetups(CardSetup[] setups)
        {
            var serialized = JsonConvert.SerializeObject(setups, Formatting.Indented);
            return serialized;
        }
    }
}