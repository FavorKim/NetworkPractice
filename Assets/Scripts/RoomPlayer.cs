using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private int _id;
    [SerializeField] private TMP_InputField _maxConnection;


    public void OnClick_StartHost()
    {
        NetworkRoomManager.singleton.maxConnections = int.Parse(_maxConnection.text);
        NetworkRoomManager.singleton.StartHost();
    }
}
