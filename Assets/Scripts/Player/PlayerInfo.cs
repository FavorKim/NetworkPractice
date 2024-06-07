using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor_Hook)),SerializeField] Color playerColor;
    [SyncVar(hook = nameof(SetName_Hook)),SerializeField] string playerName;
    [SyncVar(hook = nameof(SetImposter_Hook)),SerializeField] bool isImposter;
    [SyncVar(hook = nameof(SetIsVoted_Hook)),SerializeField] bool isVoted;
    [SyncVar(hook = nameof(SetIsVoted_Hook)),SerializeField] bool isDead;
    public MeetingPlayerPanel localPanel;

    private static PlayerInfo instance;
    public static PlayerInfo Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PlayerInfo>();
                if(instance == null)
                {
                    var obj = new GameObject("PlayerInfo");
                    obj.AddComponent<NetworkIdentity>();
                    instance = obj.AddComponent<PlayerInfo>();
                    
                    
                    DontDestroyOnLoad(instance);
                }
            }

            return instance; 
        }
    }

    public string GetName() {  return playerName; }
    public Color GetColor() { return playerColor; }
    public bool GetImposter() {  return isImposter; }
    public bool GetIsVoted() {  return isVoted; }
    public bool GetIsDead() {  return isDead; }

    public void SetColor(Color color) { playerColor = color; }
    public void SetName(string name) {  playerName = name; }
    public void SetImposter(bool val) {  isImposter = val; }
    public void SetIsVoted(bool val) { isVoted = val;}
    public void SetIsDead(bool val) { isDead = val;}

    void SetColor_Hook(Color old, Color recent)
    {
        playerColor = recent;
    }
    void SetName_Hook(string old, string recent)
    {
        playerName = recent;
    }
    void SetImposter_Hook(bool old, bool recent)
    {
        isImposter = recent;
    }
    void SetIsVoted_Hook(bool old, bool recent)
    {
        isVoted = recent;
    }
    void SetIsDead_Hook(bool old, bool recent)
    {
        isDead = recent;
    }
}
