using UnityEditor;
using UnityEngine;
using System.IO;

namespace GoodHub.Core.Editor
{
    public class MarkAllAssetsDirty : MonoBehaviour
    {
        [MenuItem("Tools/Mark All Assets Dirty")]
        public static void MarkAssetsDirty()
        {
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();
            int modifiedCount = 0;

            foreach (string path in assetPaths)
            {
                if (path.StartsWith("Assets/") == false && path.StartsWith("ProjectSettings/") == false)
                    continue; // Ignore non-project assets and folders

                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                if (!asset)
                    continue;

                EditorUtility.SetDirty(asset);
                modifiedCount++;
            }

            Debug.Log($"Marked {modifiedCount} assets as dirty. Changes will be detected on save.");
        }
    }
}