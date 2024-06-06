using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : SingletonNetworkBehaviour<GameSceneUIManager>
{
    [SerializeField] DOTweenAnimation DeadBodyReport;


    [Command(requiresAuthority = false)]
    public void CmdRpc_Report()
    {
        Rpc_Report();
    }
    [ClientRpc]
    void Rpc_Report()
    {
        DeadBodyReport.DOPlay();

    }
}
