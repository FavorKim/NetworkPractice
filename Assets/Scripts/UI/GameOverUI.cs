using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : NetworkBehaviour
{
    [SerializeField] DOTweenAnimation ImposterWin;
    [SerializeField] DOTweenAnimation CrewWin;
    DOTweenAnimation result;

    [ClientRpc]
    public void SetGameResult(bool isImposterWin)
    {
        if (!isImposterWin)
            result = CrewWin;
        else
            result = ImposterWin;
        
        DOTweenAnimation thisDo = GetComponent<DOTweenAnimation>();
        thisDo.DOPlay();
    }

    public void GameResult()
    {
        result.DOPlay();
    }
}
