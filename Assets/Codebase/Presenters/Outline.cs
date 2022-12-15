using UnityEngine;

namespace Codebase.Presenters
{
    public class Outline : MonoBehaviour
    {
        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
    }
}