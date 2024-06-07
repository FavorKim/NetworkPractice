using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChattingUI : NetworkBehaviour
{
    [SerializeField] Transform ChatHistory;
    [SerializeField] TMP_InputField Input_Msg;
    [SerializeField] MeetingTextBox TxtBoxPrefab;



    [Command(requiresAuthority =false)]
    void Cmd_SendMsg(string name, string msg, Color iconColor)
    {
        //var txtBox = Instantiate(TxtBoxPrefab,ChatHistory);
        //txtBox.SetTxtBox(name, msg, iconColor);
        Rpc_CreateTxtBox(name,msg,iconColor);
    }

    [ClientRpc]
    void Rpc_CreateTxtBox(string name, string msg, Color iconColor)
    {
        var box = Instantiate(TxtBoxPrefab,ChatHistory);
        box.SetTxtBox(name,msg, iconColor);
    }

    void SendMsg()
    {
        if (!string.IsNullOrWhiteSpace(Input_Msg.text))
            Cmd_SendMsg(PlayerInfo.Instance.GetName(), Input_Msg.text, PlayerInfo.Instance.GetColor());
    }

    public void OnClick_SendBtn()
    {
        if (!PlayerInfo.Instance.GetIsDead())
            SendMsg();
    }
}
