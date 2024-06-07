using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : SingletonNetworkBehaviour<GameSceneUIManager>
{
    [SerializeField] DeadBornReport DeadBornReport;


    [Command(requiresAuthority = false)]
    public void CmdRpc_Report(GameObject body)
    {
        Rpc_Report(body);
    }
    [ClientRpc]
    void Rpc_Report(GameObject body)
    {
        DeadBornReport.gameObject.SetActive(true);
        DeadBornReport.SetBodyColor(body);
    }
}
