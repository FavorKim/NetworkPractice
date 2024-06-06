using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonNetworkBehaviour<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    var obj = new GameObject(nameof(T));
                    obj.AddComponent<NetworkIdentity>();
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }
}
