using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImposterSetUI : MonoBehaviour
{
    [SerializeField] TMP_Text Text_Imposter;
    [SerializeField] TMP_Text Text_Crew;

    public void OnComplete_YouAre()
    {
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
