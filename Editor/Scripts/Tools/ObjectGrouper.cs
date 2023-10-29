using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace GoodHub.Core.Editor
{
    public class ObjectGrouper : EditorWindow
    {
        [MenuItem("Tools/GoodHub/Object Grouper")]
        public static void ShowExample()
        {
            ObjectGrouper window = GetWindow<ObjectGrouper>();
            window.titleContent = new GUIContent("Object Grouper");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            Button groupSelectionBtn = new Button();
            groupSelectionBtn.name = "group_selection";
            groupSelectionBtn.text = "Group Selection";

            groupSelectionBtn.clicked += GroupSelectionClicked;

            root.Add(groupSelectionBtn);
        }

        private void GroupSelectionClicked()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects == null || selectedObjects.Length == 0)
                return;

            // Find the common centre

            Vector3 centre = Vector3.zero;

            foreach (GameObject selectedObject in selectedObjects)
            {
                centre += selectedObject.transform.position / selectedObjects.Length;
            }

            // Create a new object to contain the others

            GameObject groupParent = new GameObject("Grouped Object Parent");

            groupParent.transform.position = centre;

            foreach (GameObject selectedObject in selectedObjects)
            {
                selectedObject.transform.SetParent(groupParent.transform, true);
            }
        }
    }
}