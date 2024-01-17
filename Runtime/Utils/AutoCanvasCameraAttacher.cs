using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoCanvasCameraAttacher : MonoBehaviour
{
    private void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (this == null || gameObject == null)
            return;

        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}