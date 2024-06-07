using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillBtn : MonoBehaviour
{
    [SerializeField] Button Btn_Kill;
    [SerializeField] Slider Slider_CoolTime;
    [SerializeField] TMP_Text Text_RemainingTime;
    GamePlayer Owner;

    public void InitPlayer(GamePlayer player)
    {

        Owner = player;
        Slider_CoolTime.value = 1;
        if (!Owner.GetIsImposter())
        {
            Btn_Kill.interactable = false;
            Text_RemainingTime.gameObject.SetActive(false);
            return;
        }
        Owner.OnCoolTimeReduced += SetRemainingTime;
        Owner.OnCoolTimeReduced += SetSliderCoolTime;
    }

    void SetRemainingTime()
    {
        Text_RemainingTime.text = Owner.GetCurKillCoolTime().ToString();
    }

    void SetSliderCoolTime()
    {
        Slider_CoolTime.value = Owner.GetCurKillCoolTime() / Owner.GetKillCoolTime();
    }

}
