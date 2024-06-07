using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImposterSetUI : MonoBehaviour
{
    [SerializeField] DOTweenAnimation Text_Imposter;
    [SerializeField] DOTweenAnimation Text_Crew;

    public void OnComplete_YouAre()
    {
        StartCoroutine(CorShhh());
    }

    IEnumerator CorShhh()
    {
        yield return new WaitForSeconds(2);
        if (PlayerInfo.Instance.GetImposter())
        {
            Text_Imposter.DOPlay();
        }
        else
        {
            Text_Crew.DOPlay();
        }
    }
}
