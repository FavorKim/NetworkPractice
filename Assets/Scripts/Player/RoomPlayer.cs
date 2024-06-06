using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private TMP_Text Text_Name;
    [SerializeField] private float _playerSpeed = 2f;

    bool isInited = false;
    private Vector2 _playerMoveDir = Vector2.zero;
    [SyncVar(hook = nameof(SetPlayerColor_Hook))] EPlayerColor playerColor;

    [SyncVar(hook = nameof(SetPlayerName_Hook)),SerializeField] string _playerName;

    Animator _anim;
    SpriteRenderer _spriteRenderer;



    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public override void Start()
    {
        base.Start();
        SetColor();
        if (isLocalPlayer)
        {
            var man = NetworkManager.singleton as RoomManager;
            Cmd_SetName(man.localPlayerName);

        } 
    }
    private void FixedUpdate()
    {
        Move();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            if (!isInited)
            {
                isInited = true;
                Camera.main.transform.SetParent(transform);
                Camera.main.transform.localPosition = new Vector3(0, 0, -10);
            }
        }

    }
    


    void SetColor()
    {
        var roomSlots = (NetworkManager.singleton as RoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;
        for (int i = 0; i < (int)EPlayerColor.Lime + 1; i++)
        {
            bool isSame = false;
            foreach (var roomPlayer in roomSlots)
            {
                var player = roomPlayer as RoomPlayer;
                if (player.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isSame = true;
                    break;
                }
            }
            if (!isSame)
            {
                color = (EPlayerColor)i;
                break;
            }
        }
        playerColor = color;
    }

    private void Move()
    {
        transform.position += (Vector3)_playerMoveDir;
    }

    public void OnMove(InputValue val)
    {
        if (isLocalPlayer)
        {
            Vector2 moveDir = val.Get<Vector2>();
            if (moveDir != Vector2.zero)
            {
                _playerMoveDir = moveDir * Time.deltaTime * _playerSpeed;
                _anim.SetBool("isWalk", true);
            }
            else
            {
                _playerMoveDir = Vector2.zero;
                _anim.SetBool("isWalk", false);
            }
        }
    }


    void SetPlayerColor_Hook(EPlayerColor old, EPlayerColor recent)
    {
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(recent));
    }
    void SetPlayerName_Hook(string old, string recent)
    {
        //_playerName = recent;
        Text_Name.text = recent;
    }

    [Command]
    void Cmd_SetName(string nick)
    {
        _playerName = nick;
        //man.localPlayerName = null;
        //Text_Name.text = _playerName;
    }

    //public void ReadytoBegin()
    //{
    //    CmdChangeReadyState(true);
    //}

}
