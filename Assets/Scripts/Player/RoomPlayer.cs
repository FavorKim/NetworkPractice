using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private string _id;
    [SerializeField] private TMP_InputField _maxConnection;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }



    private void Move()
    {

    }
}
