using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Codebase.Infrastructure.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Codebase.Infrastructure.Services.Factories
{
    public class CardIconFactory : IService
    {
        public bool IsLoaded => _isLoaded;

        private const int Height = 300;
        private const int Width = 300;
        private static readonly string ImageGeneratorUri = $"https://picsum.photos/{Height}/{Width}";
        private const int DefaultPoolSize = 5;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<Sprite> _spritesPool;
        private readonly List<Texture2D> _textures;
        private bool _isLoaded;

        public CardIconFactory(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _textures = new List<Texture2D>(DefaultPoolSize);
            _spritesPool = new List<Sprite>(DefaultPoolSize);
            _isLoaded = false;
            LoadSpritesFromWeb();
        }

        public Sprite GetRandomIcon()
        {
            return _spritesPool[Random.Range(0, _spritesPool.Count)];
        }

        private void LoadSpritesFromWeb()
        {
            _coroutineRunner.StartCoroutine(LoadImages());
        }

        private IEnumerator LoadImages()
        {
            for (int i = 0; i < DefaultPoolSize; i++)
            {
                UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(ImageGeneratorUri);
                yield return LoadImage(webRequest);
            }
            ConvertTexturesToSprites(_textures.ToArray(), _spritesPool);
            
            _isLoaded = true;
        }

        private IEnumerator LoadImage(UnityWebRequest webRequest)
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    _textures.Add(DownloadHandlerTexture.GetContent(webRequest));
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    throw new WebException("Connection error occurred during web request.");
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                default:
                    throw new Exception("Unhandled exception during web request.");
            }

            yield return null;
        }

        private void ConvertTexturesToSprites(Texture2D[] textures, List<Sprite> sprites)
        {
            foreach (var t in textures)
            {
                sprites.Add(ConvertToSprite(t));
            }
        }

        private Sprite ConvertToSprite(Texture2D texture)
        {
            var sprite = Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}