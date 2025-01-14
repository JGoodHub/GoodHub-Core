using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor
{
    public class SpriteImportSettingsCopier : EditorWindow
    {
        [SerializeField] private Sprite _sourceSprite;
        [SerializeField] private List<Sprite> _targetSprites = new List<Sprite>();

        private Vector2 _scrollPosition;

        [MenuItem("Tools/GoodHub/Sprite Import Settings Copier")]
        public static void ShowWindow()
        {
            GetWindow<SpriteImportSettingsCopier>("Sprite Import Settings Copier");
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            GUILayout.Label("Source Sprite", EditorStyles.boldLabel);

            _sourceSprite = (Sprite)EditorGUILayout.ObjectField("Source Sprite", _sourceSprite, typeof(Sprite), false);

            GUILayout.Label("Target Sprites", EditorStyles.boldLabel);

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty targetSpritesProperty = serializedObject.FindProperty("_targetSprites");

            EditorGUILayout.PropertyField(targetSpritesProperty, new GUIContent("Target Sprites"), true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Copy Import Settings"))
            {
                CopyImportSettings();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void CopyImportSettings()
        {
            if (_sourceSprite == null)
            {
                Debug.LogError("Source sprite is not assigned.");
                return;
            }

            if (_targetSprites.Count == 0)
            {
                Debug.LogError("No target sprites assigned.");
                return;
            }

            string sourcePath = AssetDatabase.GetAssetPath(_sourceSprite);
            TextureImporter sourceImporter = AssetImporter.GetAtPath(sourcePath) as TextureImporter;

            if (sourceImporter == null)
            {
                Debug.LogError("Failed to get TextureImporter for the source sprite.");
                return;
            }

            foreach (Sprite targetSprite in _targetSprites)
            {
                if (targetSprite == null)
                    continue;

                string targetPath = AssetDatabase.GetAssetPath(targetSprite);
                TextureImporter targetImporter = AssetImporter.GetAtPath(targetPath) as TextureImporter;

                if (targetImporter == null)
                {
                    Debug.LogError($"Failed to get TextureImporter for target sprite: {targetSprite.name}");
                    continue;
                }

                // Copy sprite-related settings
                targetImporter.textureType = sourceImporter.textureType;
                targetImporter.spriteImportMode = sourceImporter.spriteImportMode;
                targetImporter.spritePixelsPerUnit = sourceImporter.spritePixelsPerUnit;
                targetImporter.spritePivot = sourceImporter.spritePivot;
                targetImporter.wrapMode = sourceImporter.wrapMode;
                targetImporter.filterMode = sourceImporter.filterMode;
                targetImporter.spriteBorder = sourceImporter.spriteBorder;

                // Apply changes
                AssetDatabase.ImportAsset(targetPath, ImportAssetOptions.ForceUpdate);
            }

            Debug.Log("Import settings copied successfully.");
        }
    }
}