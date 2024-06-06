using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor_Hook)),SerializeField] Color playerColor;
    [SyncVar(hook = nameof(SetName_Hook)),SerializeField] string playerName;
    [SyncVar(hook = nameof(SetImposter_Hook)),SerializeField] bool isImposter;

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


    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    if (instance != null)
    //        DestroyImmediate(instance.gameObject);
    //    instance = this;
    //    //DontDestroyOnLoad(gameObject);
    //}


    public string GetName() {  return playerName; }
    public Color GetColor() { return playerColor; }
    public bool GetImposter() {  return isImposter; }

    public void SetColor(Color color) { playerColor = color; }
    public void SetName(string name) {  playerName = name; }
    public void SetImposter(bool val) {  isImposter = val; }

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
}
