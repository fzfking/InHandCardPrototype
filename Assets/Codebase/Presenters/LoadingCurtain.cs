using TMPro;
using UnityEngine;

namespace Codebase.Presenters
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Label;

        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);

        public void UpdateStatus(string text)
        {
            Label.text = text;
        }
    }
}