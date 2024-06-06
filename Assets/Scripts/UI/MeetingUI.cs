using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : NetworkBehaviour
{
    [SerializeField] GameObject PlayerPanelPref;
    [SerializeField] GridLayoutGroup GridLayout_Players;
    List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();


    

    [Command(requiresAuthority = false), ClientRpc]
    public void CmdRpc_OnOpenMeeting()
    {
        foreach (GamePlayer player in GameManager.gamePlayers)
        {
            MeetingPlayerPanel panel = Instantiate(PlayerPanelPref, GridLayout_Players.transform).GetComponent<MeetingPlayerPanel>();
            panel.SetVoter(player);
            meetingPlayerPanels.Add(panel);
        }
    }

    [Server]
    GamePlayer GetEjectedPlayer()
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
        return ejectedPlayer;
    }

    [ClientRpc]
    void BanPlayer(GamePlayer ejectedPlayer)
    {
        ejectedPlayer.RpcOnKilled();
        Debug.Log(ejectedPlayer.GetName() + "is Ejected");
    }

    //[Server]
    public void OnEndMeeting()
    {
        foreach(MeetingPlayerPanel panel in meetingPlayerPanels)
        {
            panel.OnMeetingEnd();
        }
        StartCoroutine(CorBan());
        
    }

    IEnumerator CorBan()
    {
        yield return new WaitForSeconds(3.0f);
        BanPlayer(GetEjectedPlayer());
        foreach(MeetingPlayerPanel panel in meetingPlayerPanels)
        {
            Destroy(panel.gameObject);
        }
        meetingPlayerPanels.Clear();
    }


    public void OnValueChanged_TimeBar(Single val)
    {
        if (val == 0) OnEndMeeting();
    }
}
