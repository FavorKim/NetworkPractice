using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class RoomUI : NetworkBehaviour
{
    [SerializeField] TMP_Text Text_People;
    StringBuilder _peoples = new();


    private void Update()
    {
        ShowConnectedPlayers();
    }

    void ShowConnectedPlayers()
    {
        Text_People.text = $"{NetworkManager.singleton.numPlayers} / {NetworkManager.singleton.maxConnections}";
    }

    public void OnClick_ReadyBtn(bool value)
    {
        
    }

    void ClientReadyToBegin()
    {

    }
}
