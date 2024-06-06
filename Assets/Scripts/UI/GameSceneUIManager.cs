using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : SingletonNetworkBehaviour<GameSceneUIManager>
{
    [SerializeField] DOTweenAnimation DeadBodyReport;


    [Command(requiresAuthority = false),ClientRpc]
    public void CmdRpc_Report()
    {
        DeadBodyReport.DOPlay();
    }
}
