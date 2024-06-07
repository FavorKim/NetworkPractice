using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    public static List<GamePlayer> gamePlayers = new List<GamePlayer>();


    static int _nonImposters;
    static int _imposterCount;

    public override void OnStartServer()
    {
        if (instance != null && instance != this)
            DestroyImmediate(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        _nonImposters = NetworkServer.connections.Count;
        _imposterCount = NetworkServer.connections.Count / 5 + 1;

        Invoke("SetImposters", 1.0f);
    }


    [Server]
    void SetImposters()
    {

        foreach (GamePlayer player in gamePlayers)
        {
            player.RpcSetImposter(GetRandomImposter());
        }
    }
    [Server]
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
    public override void OnStopServer()
    {
        gamePlayers.Clear();
    }

    [Command(requiresAuthority = false)]
    public void Cmd_BodyClean()
    {
        var bodies = GameObject.FindGameObjectsWithTag("DeadBody");
        foreach (var item in bodies)
        {
            NetworkServer.UnSpawn(item);
        }
    }
}
