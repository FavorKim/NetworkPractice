using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    

    public void OnClick_CreateRoom()
    {
        NetworkManager.singleton.StartHost();
    }
    public void OnClick_JoinRoom()
    {
        NetworkManager.singleton.StartClient();
    }
}
