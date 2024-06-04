using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : NetworkRoomManager
{


    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
    }


}
