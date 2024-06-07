using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : NetworkBehaviour
{
    [SerializeField] GameObject PlayerPanelPref;
    [SerializeField] GridLayoutGroup GridLayout_Players;
    List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();



    public void OnOpenMeeting()
    {
        foreach (GamePlayer player in GameManager.gamePlayers)
        {
            MeetingPlayerPanel panel = Instantiate(PlayerPanelPref, GridLayout_Players.transform).GetComponent<MeetingPlayerPanel>();
            panel.Rpc_SetVoter(player);
            meetingPlayerPanels.Add(panel);
            if(player.isLocalPlayer) PlayerInfo.Instance.localPanel = panel;
        }
        
    }

    
    //[Server]
    GameObject GetEjectedPlayer()
    {
        int votedHigh = 0;
        GamePlayer ejectedPlayer = null;
        foreach(GamePlayer player in GameManager.gamePlayers)
        {
            if (player._IsDead) continue;
            if(player.GetVotedNum() > votedHigh)
            {
                votedHigh = player.GetVotedNum();
                ejectedPlayer = player;
            }
        }
        return ejectedPlayer.gameObject;
    }

    [ClientRpc]
    void Rpc_BanPlayer(GameObject ejectedPlayer)
    {
        Debug.Log(ejectedPlayer.GetComponent<GamePlayer>().GetName() + "is Ejected");
    }

    [Command(requiresAuthority =false)]
    void Cmd_BanPlayer(GameObject ejectedPlayer)
    {
        ejectedPlayer.GetComponent<GamePlayer>().RpcOnKilled();
        Rpc_BanPlayer(ejectedPlayer);
    }



    [Command(requiresAuthority =false)]
    void Cmd_OnEndMeeting()
    {
        Rpc_OnEndMeeting();
        GameManager.Instance.Cmd_BodyClean();
    }

    [ClientRpc]
    void Rpc_OnEndMeeting()
    {
        foreach (MeetingPlayerPanel panel in meetingPlayerPanels)
        {
            panel.OnMeetingEnd();
        }
        Invoke(nameof(CloseMeeting), 3.0f);
    }

    //[ClientRpc]
    void CloseMeeting()
    {
        //yield return new WaitForSeconds(3.0f);
        Cmd_BanPlayer(GetEjectedPlayer());
        ResetPanels();
        meetingPlayerPanels.Clear();
        this.gameObject.SetActive(false);
    }

    void ResetPanels()
    {
        foreach (MeetingPlayerPanel panel in meetingPlayerPanels)
        {
            Destroy(panel.gameObject);
        }
        
    }

    //[Command(requiresAuthority =false)]
    public void OnValueChanged_TimeBar(Single val)
    {
        if (val == 0) 
        {
            GameManager.Instance.Cmd_BodyClean();
            Cmd_OnEndMeeting(); 
        }
    }
}
