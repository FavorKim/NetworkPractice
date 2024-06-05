using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayer : NetworkBehaviour
{
    CharacterController _controller;
    [SerializeField] GameObject Gameobject_PlayerHead;
    [SerializeField] GameObject Collider_ImposterKillRange;

    [SerializeField] bool _isImposter;
    [SerializeField] bool _canKill;

    Vector3 _moveDir;
    Vector2 _dir;

    [SyncVar, SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;



    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        var cam = GameObject.Find("Cam_FPS");
        cam.transform.SetParent(Gameobject_PlayerHead.transform);
        cam.transform.localPosition = Vector3.zero;
    }

    public override void OnStartServer()
    {
        GameManager.gamePlayers.Add(this);
    }


    #region Local
    void SetImposterKillRange()
    {
        if (this.isLocalPlayer && _isImposter)
            Collider_ImposterKillRange.SetActive(true);
    }
    void MovePlayer()
    {
        if (isLocalPlayer)
        {
            _moveDir = Gameobject_PlayerHead.transform.TransformDirection(new Vector3(_dir.x, 0, _dir.y)) * _moveSpeed * Time.deltaTime;
            _controller.SimpleMove(_moveDir);
        }
    }
    #endregion

    #region RPCs
    [ClientRpc]
    public void RpcSetImposter(bool isImposter)
    {
        this._isImposter = isImposter;
        SetImposterKillRange();
    }

    [ClientRpc]
    public void RpcOnKilled()
    {
        Debug.Log(netId+"is Killed");
        gameObject.SetActive(false);
    }
    
    #endregion

    #region Command
    [Command]
    public void KillCommand()
    {
        Debug.Log("killcmd");
        RpcOnKilled();
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

    
    #endregion

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("col");
            other.transform.root.GetComponent<GamePlayer>()?.KillCommand();
        }
    }
}

