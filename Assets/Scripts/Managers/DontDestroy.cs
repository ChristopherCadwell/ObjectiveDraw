using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private static GameObject instance;

    public static GameObject Instance => instance;

    public static bool IsInitialized => instance != null;

    private void Awake()
    {
        if (IsInitialized)
            Destroy(gameObject);
        else
            instance = gameObject;

        DontDestroyOnLoad(gameObject);
    }

    public void CheckDelete()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        if (buildIndex == (int)SceneIndexes.MAIN_MENU)
            Destroy(gameObject);
    }
}
