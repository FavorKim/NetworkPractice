using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField, SyncVar(hook = "InitID")] private string _id;
    [SerializeField] private TMP_Text Text_Name;

    [SerializeField] private float _playerSpeed = 2f;
    Animator _anim;
    bool isInited = false;

    private Vector2 _playerMoveDir = Vector2.zero;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer && !isInited)
        {
            isInited = true;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (Vector3)_playerMoveDir;
    }

    public void OnMove(InputValue val)
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

    public void InitID(string _, string value)
    {
        Text_Name.text = value;
    }

    //public void ReadytoBegin()
    //{
    //    CmdChangeReadyState(true);
    //}

}
