using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerVoter : MonoBehaviour
{
    [SerializeField] TMP_Text Text_Playername;
    [SerializeField] Image Img_PlayerColor;

    public void SetVoter(string playername, Color color)
    {
        Text_Playername.text = playername;
        Img_PlayerColor.color = color;
    }
}
