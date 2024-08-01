using UnityEngine;
using Photon.Pun;

public class SingletonTemplatePunCallbacks<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T instance = null;

    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance)
        {
            Debug.LogWarning($"More than one {typeof(T)} instance found");
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        name += " [MANAGER]";
        DontDestroyOnLoad(gameObject);
    }
}