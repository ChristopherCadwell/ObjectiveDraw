using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton<T> : MonoBehaviour where T : ManagerSingleton<T>
{
    private static T instance;
    public static T Instance => instance;

    public static bool IsInitialized => instance != null;

    protected virtual void Awake()
    {
        if (IsInitialized)
            Destroy(gameObject);
        else
            instance = (T)this;

        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}