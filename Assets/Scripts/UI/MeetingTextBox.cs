using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeetingTextBox : MonoBehaviour
{
    [SerializeField] Image Img_Icon;
    [SerializeField] TMP_Text Text_Msg;


    public void SetTxtBox(string name, string msg, Color iconColor) 
    {
        Material mat = Instantiate(Img_Icon.material);
        mat.SetColor("_PlayerColor", iconColor);
        Img_Icon.material = mat;
        Text_Msg.text = ($"{name} : {msg}").Trim();
    }
}
