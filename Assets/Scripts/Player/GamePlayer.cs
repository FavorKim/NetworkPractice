using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayer : NetworkBehaviour
{
    CharacterController _controller;
    [SerializeField] GameObject PlayerHead;

    Vector3 _moveDir;

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

    void MovePlayer()
    {
        _controller.SimpleMove(_moveDir);
    }

    public void OnMove(InputValue val)
    {
        Vector2 dir = val.Get<Vector2>();
        _moveDir = new Vector3(dir.x, 0, dir.y) * _moveSpeed * Time.deltaTime;
    }

    public void OnRotate(InputValue val)
    {
        Vector2 delta = val.Get<Vector2>();
        PlayerHead.transform.Rotate(delta * _rotateSpeed * Time.deltaTime);
    }
}
