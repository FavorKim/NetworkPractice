using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayer : NetworkBehaviour
{
    CharacterController _controller;
    [SerializeField] GameObject PlayerHead;

    [SerializeField] bool isImposter;

    Vector3 _moveDir;
    Vector2 dir;

    [SyncVar, SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;



    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        MovePlayer();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //if (!isLocalPlayer) return;
        //Camera.main.transform.SetParent(PlayerHead.transform);
        var cam = GameObject.Find("Cam_FPS");
        cam.transform.SetParent(PlayerHead.transform);
        cam.transform.localPosition = Vector3.zero;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.gamePlayers.Add(this);
    }


    #region ServerSide

    #endregion

    #region ClientSide
    void MovePlayer()
    {
        _moveDir = PlayerHead.transform.TransformDirection(new Vector3(dir.x, 0, dir.y)) * _moveSpeed * Time.deltaTime;
        _controller.SimpleMove(_moveDir);
    }

    #region RPCs
    [ClientRpc]
    public void SetImposter(bool isImposter)
    {
        Debug.Log("setimposter");
        this.isImposter = isImposter;
    }

    #endregion
    #endregion

    #region Events
    public void OnMove(InputValue val)
    {
        if (!isLocalPlayer) return;
        dir = val.Get<Vector2>();
    }
    public void OnRotate(InputValue val)
    {
        if (!isLocalPlayer) return;
        Vector2 delta = val.Get<Vector2>();
        Vector2 rotateVector = new Vector2(-delta.y, delta.x);
        PlayerHead.transform.Rotate(rotateVector * _rotateSpeed * Time.deltaTime);
        PlayerHead.transform.eulerAngles = new Vector3(PlayerHead.transform.eulerAngles.x, PlayerHead.transform.eulerAngles.y, 0);
    }
    public void OnRecieveMsg(NetworkConnectionToClient conn, bool msg)
    {

    }
    #endregion
}

public class ImposterSender : NetworkMessage
{
    bool isImposter;
    public ImposterSender() { }
    public ImposterSender(bool isImposter)
    {
        this.isImposter = isImposter;
    }
}
