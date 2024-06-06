using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MeetingPlayerPanel : NetworkBehaviour
{
    [SerializeField] TMP_Text Text_Playername;
    [SerializeField] Image Img_PlayerColor;
    [SerializeField] Image Img_Dead;

    [SerializeField] GameObject VotedPref;
    [SerializeField] GridLayoutGroup Group_Voted;
    GamePlayer player;

    //[Command(requiresAuthority =false)]
    public void Cmd_SetVoter()
    {
        Rpc_SetVoter(player);
    }

    //[ClientRpc]
    public void Rpc_SetVoter(GamePlayer player)
    {
        Material mate = Instantiate(Img_PlayerColor.material);
        Img_PlayerColor.material = mate;
        this.player = player;
        Text_Playername.text = player.GetName();
        Img_PlayerColor.material.SetColor("_PlayerColor", player.GetPlayerColor());
        Img_Dead.gameObject.SetActive(player._IsDead);

        if (player._IsDead)
        {
            var btn = GetComponent<Button>();
            btn.interactable = false;
        }
    }


    //[Command(requiresAuthority = false)]
    public void OnClick_Panel()
    {
        player.Voted();
    }

    [Command(requiresAuthority = false)]
    public void OnMeetingEnd()
    {
        Rpc_OnMeetingEnd();
    }
    [ClientRpc]
    void Rpc_OnMeetingEnd()
    {
        for (int i = 0; i < player.GetVotedNum(); i++)
        {
            var votedObj = Instantiate(VotedPref, Group_Voted.transform);
        }
    }
}
