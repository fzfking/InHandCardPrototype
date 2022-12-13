using UnityEditor;
using UnityEngine;

namespace Codebase.EditorTools.Editor
{
    [CustomEditor(typeof(CardSetupsEditor))]
    public class CardSetupsCustomEditor : UnityEditor.Editor
    {
        private CardSetupsEditor _editor;
        public override void OnInspectorGUI()
        {
            _editor = (CardSetupsEditor)target;
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Load"))
            {
                _editor.Load();
            }

            if (GUILayout.Button("Save"))
            {
                _editor.Save();
            }
        }
    }
}