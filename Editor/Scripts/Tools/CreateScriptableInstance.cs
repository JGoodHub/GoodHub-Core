using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor
{
    public static class CreateScriptableInstance
    {
        [MenuItem("Assets/GoodHub/Create ScriptableObject Instance", true)]
        private static bool ValidateCreateScriptableObjectInstance()
        {
            // Ensure the selected asset is a C# script
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return path.EndsWith(".cs");
        }

        [MenuItem("Assets/GoodHub/Create ScriptableObject Instance")]
        private static void CreateScriptableObjectInstance()
        {
            string scriptPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string scriptName = System.IO.Path.GetFileNameWithoutExtension(scriptPath);

            // Find the type of the ScriptableObject
            System.Type type = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.Name == scriptName && typeof(ScriptableObject).IsAssignableFrom(t));

            if (type == null)
            {
                Debug.LogError($"No ScriptableObject type found matching the name: {scriptName}");
                return;
            }

            // Create an instance of the ScriptableObject
            ScriptableObject instance = ScriptableObject.CreateInstance(type);

            // Save the instance to an asset file
            string folderPath = System.IO.Path.GetDirectoryName(scriptPath);
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{scriptName}.asset");
            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"ScriptableObject instance of type {scriptName} created at {assetPath}");
        }

    }
}