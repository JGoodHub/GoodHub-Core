using GoodHub.Core.Runtime;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor
{

    [CustomEditor(typeof(ScriptableSingletonViewer))]
    public class ScriptableSingletonViewerInspector : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            GUIStyle _headerStyle = new GUIStyle(EditorStyles.boldLabel);
            _headerStyle.alignment = TextAnchor.MiddleCenter;
            _headerStyle.fontSize = 14;
            
            Debug.Log(name);

            base.OnInspectorGUI();

            ScriptableSingletonViewer viewer = (ScriptableSingletonViewer) target;

            EditorGUILayout.Space();
            
            if (viewer._scriptableObject == null)
                return;

            EditorGUILayout.LabelField(viewer._scriptableObject.GetType().ToString(), _headerStyle);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            UnityEditor.Editor editor = CreateEditor(viewer._scriptableObject);
            editor.OnInspectorGUI();
        }

    }

}