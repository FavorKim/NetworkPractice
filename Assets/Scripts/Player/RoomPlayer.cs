using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private string _id;
    [SerializeField] private TMP_InputField _maxConnection;

    [SyncVar, SerializeField] private float _playerSpeed = 2f;
    Animator _anim;

    private Vector2 _playerMoveDir;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }


    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(_playerMoveDir);
    }

    public void OnMove(InputValue val)
    {
        Vector2 moveDir = val.Get<Vector2>();
        if (moveDir != Vector2.zero)
        {
            _playerMoveDir = moveDir * _playerSpeed * Time.deltaTime;
            _anim.SetBool("isWalk", true);
        }
        else
        {
            _playerMoveDir = Vector2.zero;
            _anim.SetBool("isWalk", false);
        }
    }

}
