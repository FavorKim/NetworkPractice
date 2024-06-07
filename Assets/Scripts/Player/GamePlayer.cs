using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayer : NetworkBehaviour
{
    CharacterController _controller;
    [SerializeField] GameObject Gameobject_PlayerHead;
    [SerializeField] GameObject Prefab_DeadBody;
    [SerializeField] TMP_Text Text_PlayerName;

    [SerializeField] bool _isImposter;
    [SerializeField] bool _canKill;
    [SerializeField] LayerMask LayerMask_Player;
    [SerializeField] LayerMask LayerMask_Body;
    [SerializeField] public bool _IsDead { get; set; }
    [SerializeField, SyncVar(hook = nameof(SetVotedNum_Hook))] int _votedNumber = 0;
    [SerializeField, SyncVar(hook = nameof(SetIsVoted_Hook))] private bool _isVoted;

    Vector3 _moveDir;
    Vector2 _dir;

    [SyncVar, SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    [SyncVar(hook = nameof(SetName_Hook))] string _playerName;
    [SyncVar(hook = nameof(SetColor_Hook))] Color _playerColor;

    MeshRenderer _renderer;

    public bool GetIsImposter() { return _isImposter; }
    public string GetName() { return _playerName; }
    public Color GetPlayerColor() { return _playerColor; }
    public int GetVotedNum() { return _votedNumber; }
    public bool GetIsVoted() { return _isVoted; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Start()
    {
        GameManager.gamePlayers.Add(this);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        var cam = GameObject.Find("Cam_FPS");
        cam.transform.SetParent(Gameobject_PlayerHead.transform);
        cam.transform.localPosition = Vector3.zero;
        Cmd_SetName(PlayerInfo.Instance.GetName());
        Cmd_SetColor(PlayerInfo.Instance.GetColor());
    }


    #region Local
    void MovePlayer()
    {
        if (isLocalPlayer)
        {
            _moveDir = Gameobject_PlayerHead.transform.TransformDirection(new Vector3(_dir.x, 0, _dir.y)) * _moveSpeed * Time.deltaTime;
            _controller.SimpleMove(_moveDir);
        }
    }

    GamePlayer RaycastGetPlayer()
    {
        RaycastHit[] hit = null;
        GamePlayer dest = null;
        hit = Physics.RaycastAll(Gameobject_PlayerHead.transform.position, Gameobject_PlayerHead.transform.forward, 3.0f, LayerMask_Player);
        foreach (var r in hit)
        {
            if (r.collider == this) continue;
            dest = r.collider.GetComponent<GamePlayer>();
        }

        return dest;
    }

    //[Command]
    void RayCastBody()
    {
        if (Physics.Raycast(Gameobject_PlayerHead.transform.position, Gameobject_PlayerHead.transform.forward, 4.0f, LayerMask_Body))
        {
            GameSceneUIManager.Instance.CmdRpc_Report();
        }
    }



    #endregion

    #region RPCs
    [ClientRpc]
    public void RpcSetImposter(bool isImposter)
    {
        this._isImposter = isImposter;
        if (isLocalPlayer) PlayerInfo.Instance.SetImposter(isImposter);
    }

    [ClientRpc]
    public void RpcOnKilled()
    {
        _IsDead = true;
    }
    [ClientRpc]
    void Rpc_SetBodyColor(GameObject body, GamePlayer killed)
    {
        body.GetComponent<MeshRenderer>().material.color = killed._playerColor;
    }

    #endregion

    #region Command
    [Command(requiresAuthority = false)]
    public void KillCommand()
    {
        var body = Instantiate(Prefab_DeadBody, transform.position, Prefab_DeadBody.transform.rotation);
        NetworkServer.Spawn(body);
        Rpc_SetBodyColor(body, this);
        RpcOnKilled();
    }
    [Command(requiresAuthority = false)]
    public void Cmd_SetName(string name)
    {
        _playerName = name;
    }
    [Command(requiresAuthority = false)]
    public void Cmd_SetColor(Color color)
    {
        _playerColor = color;
    }


    #endregion

    #region Events
    public void OnMove(InputValue val)
    {
        if (!isLocalPlayer) return;
        _dir = val.Get<Vector2>();
    }
    public void OnRotate(InputValue val)
    {
        if (!isLocalPlayer) return;
        Vector2 delta = val.Get<Vector2>();
        Vector2 rotateVector = new Vector2(-delta.y, delta.x);
        Gameobject_PlayerHead.transform.Rotate(rotateVector * _rotateSpeed * Time.deltaTime);
        Gameobject_PlayerHead.transform.eulerAngles = new Vector3(Gameobject_PlayerHead.transform.eulerAngles.x, Gameobject_PlayerHead.transform.eulerAngles.y, 0);
    }
    public void OnKill(InputValue val)
    {
        if (_isImposter)
        {
            RaycastGetPlayer().KillCommand();
        }
    }
    public void OnReport(InputValue val)
    {
        RayCastBody();
    }

    #endregion

    void SetName_Hook(string old, string recent)
    {
        Text_PlayerName.text = recent;
    }
    void SetColor_Hook(Color old, Color recent)
    {
        if (_renderer == null) _renderer = GetComponent<MeshRenderer>();
        _renderer.material.color = recent;
    }
    void SetVotedNum_Hook(int old, int recent)
    {
        _votedNumber = recent;
    }
    void SetIsVoted_Hook(bool old, bool recent)
    {
        _isVoted = recent;
    }

    public void Voted()
    {
        _votedNumber++;
        _isVoted = true;
    }
    public void ResetVote()
    {
        _votedNumber = 0;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        other.transform.root.GetComponent<GamePlayer>()?.KillCommand();
    //    }
    //}
}

