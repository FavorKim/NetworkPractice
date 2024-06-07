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
    public GamePlayer Owner;


}
