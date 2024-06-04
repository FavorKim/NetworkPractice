using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    [SerializeField] public static List<GamePlayer> gamePlayers = new List<GamePlayer>();


    [SerializeField] static int _nonImposters;
    [SerializeField] static int _imposterCount;

    void Start()
    {
        if (instance != null && instance != this)
            DestroyImmediate(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        _nonImposters = NetworkServer.connections.Count;
        _imposterCount = NetworkServer.connections.Count / 5 + 1;

        Invoke("SetImposters", 1.5f);
    }


    [Server]
    void SetImposters()
    {
        foreach(GamePlayer player in gamePlayers)
        {
            player.RpcSetImposter(GetRandomImposter());
        }
    }

    bool GetRandomImposter()
    {
        if (_imposterCount == 0) return false;

        int index = Random.Range(0, _nonImposters);
        _nonImposters--;
        if (index < _imposterCount)
        {
            _imposterCount--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
