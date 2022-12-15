using UnityEngine;

namespace Codebase.Infrastructure.Services.Loaders
{
    public static class GenericResourcesLoader
    {
        public static TResource Load<TResource>(string path) where TResource: Object
        {
            return Resources.Load<TResource>(path);
        }
    }
}