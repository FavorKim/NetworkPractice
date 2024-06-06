using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text Text_Playername;
    [SerializeField] Image Img_PlayerColor;
    [SerializeField] Image Img_Dead;
    [SerializeField] GameObject VotedPref;
    [SerializeField] GridLayoutGroup Group_Voted;
    GamePlayer player;

    public void SetVoter(GamePlayer player)
    {
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

    public void OnClick_Panel()
    {
        player.Voted();
    }

    public void OnMeetingEnd()
    {
        for(int i=0; i<player.GetVotedNum(); i++)
        {
            var votedObj = Instantiate(VotedPref, Group_Voted.transform);
        }
    }
}
