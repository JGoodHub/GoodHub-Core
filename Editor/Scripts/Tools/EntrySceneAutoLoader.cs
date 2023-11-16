using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene auto loader.
/// </summary>
/// <description>
/// This class adds a File > Scene Autoload menu containing options to select
/// a "Entry scene" enable it to be auto-loaded when the user presses play
/// in the editor. When enabled, the selected scene will be loaded on play,
/// then the original scene will be reloaded on stop.
///
/// Based on an idea on this thread:
/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
/// </description>
[InitializeOnLoad]
internal static class EntrySceneAutoLoader
{
    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets executed.
    static EntrySceneAutoLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    // Menu items to select the "Entry" scene and control whether or not to load it.
    [MenuItem("File/Scene Autoload/Select Entry Scene...")]
    private static void SelectEntryScene()
    {
        string entryScene = EditorUtility.OpenFilePanel("Select Entry Scene", Application.dataPath, "unity");
        entryScene = entryScene.Replace(Application.dataPath, "Assets"); //project relative instead of absolute path

        if (string.IsNullOrEmpty(entryScene))
            return;
        
        EntryScene = entryScene;
        LoadEntryOnPlay = true;
    }


    [MenuItem("File/Scene Autoload/Load 'Entry' scene On Play", true)]
    private static bool ShowLoadEntryOnPlay()
    {
        return LoadEntryOnPlay == false;
    }

    [MenuItem("File/Scene Autoload/Load 'Entry' scene On Play")]
    private static void EnableLoadEntryOnPlay()
    {
        LoadEntryOnPlay = true;
    }

    [MenuItem("File/Scene Autoload/Don't Load 'Entry' scene On Play", true)]
    private static bool ShowDontLoadEntryOnPlay()
    {
        return LoadEntryOnPlay;
    }

    [MenuItem("File/Scene Autoload/Don't Load 'Entry' scene On Play")]
    private static void DisableLoadEntryOnPlay()
    {
        LoadEntryOnPlay = false;
    }

    // Play mode change callback handles the scene load/reload.
    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (LoadEntryOnPlay == false ||
            EditorApplication.isPlaying)
            return;

        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // User pressed play -- autoload Entry scene.
            PreviousScene = SceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene(EntryScene);
                }
                catch
                {
                    Debug.LogError($"Error: scene not found: {EntryScene}");
                    EditorApplication.isPlaying = false;
                }
            }
            else
            {
                // User cancelled the save operation -- cancel play as well.
                EditorApplication.isPlaying = false;
            }
        }
        else
        {
            // User pressed stop -- reload previous scene.
            try
            {
                EditorSceneManager.OpenScene(PreviousScene);
            }
            catch
            {
                Debug.LogError($"Error: scene not found: {PreviousScene}");
            }
        }
    }

    // Properties are remembered as editor preferences.
    private const string _EditorPrefLoadEntryOnPlay = "SceneAutoEntry.LoadEntryOnPlay";
    private const string _EditorPrefEntryScene = "SceneAutoEntry.EntryScene";
    private const string _EditorPrefPreviousScene = "SceneAutoEntry.PreviousScene";

    private const string _DefaultEntryScenePath = "Assets/0_Scenes/Entry.unity";

    private static bool LoadEntryOnPlay
    {
        get => EditorPrefs.GetBool(_EditorPrefLoadEntryOnPlay, true);
        set => EditorPrefs.SetBool(_EditorPrefLoadEntryOnPlay, value);
    }

    private static string EntryScene
    {
        get => EditorPrefs.GetString(_EditorPrefEntryScene, "Entry.unity");
        set => EditorPrefs.SetString(_EditorPrefEntryScene, value);
    }

    private static string PreviousScene
    {
        get => EditorPrefs.GetString(_EditorPrefPreviousScene, SceneManager.GetActiveScene().path);
        set => EditorPrefs.SetString(_EditorPrefPreviousScene, value);
    }
}