using System.IO;
using Codebase.DataTransferObjects;
using Codebase.Infrastructure.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Loaders
{
    public class CardSetupsLoader: IService
    {
        public static readonly string CardSetupsPath = $"{Application.streamingAssetsPath}/Setup/Cards.json";
        private readonly CardSetup[] _setups;

        public CardSetupsLoader()
        {
            _setups = LoadSetups();
        }

        public CardSetup[] GetSetups() => _setups;

        private CardSetup[] LoadSetups()
        {
            var serialized = GetJsonCardSetups();
            return DeserializeCardSetups(serialized);
        }

        private string GetJsonCardSetups()
        {
            string serialized;
            using (StreamReader streamReader = new StreamReader(CardSetupsPath))
            {
                serialized = streamReader.ReadToEnd();
            }

            return serialized;
        }

        private CardSetup[] DeserializeCardSetups(string serialized)
        {
            var deserialized = JsonConvert.DeserializeObject<CardSetup[]>(serialized);
            return deserialized;
        }
    }
}