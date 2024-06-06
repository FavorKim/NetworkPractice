using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text Text_Playername;
    [SerializeField] Image Img_PlayerColor;
    [SerializeField] Image Img_Reporter;
    [SerializeField] Image Img_Dead;
    [SerializeField] Image Img_IVoted;

    public void SetVoter(string playername, Color color)
    {
        Text_Playername.text = playername;
        Img_PlayerColor.material.SetColor("_PlayerColor", color);
    }
}
