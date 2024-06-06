using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : MonoBehaviour
{
    [SerializeField] GameObject PlayerPanelPref;
    [SerializeField] GridLayoutGroup GridLayout_Players;

    public void OnOpenMeeting()
    {
        foreach(GamePlayer player in GameManager.gamePlayers)
        {
            MeetingPlayerPanel panel = Instantiate(PlayerPanelPref, GridLayout_Players.transform).GetComponent<MeetingPlayerPanel>();

        }
    }

}
