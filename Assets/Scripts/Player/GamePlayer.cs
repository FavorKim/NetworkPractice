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
    Vector2 dir;

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
        _moveDir = PlayerHead.transform.TransformDirection(new Vector3(dir.x, 0, dir.y)) * _moveSpeed * Time.deltaTime;
        _controller.SimpleMove(_moveDir);
    }

    public void OnMove(InputValue val)
    {
        dir = val.Get<Vector2>();
    }

    public void OnRotate(InputValue val)
    {
        Vector2 delta = val.Get<Vector2>();
        Vector2 rotateVector = new Vector2(-delta.y, delta.x);
        PlayerHead.transform.Rotate(rotateVector * _rotateSpeed * Time.deltaTime);
        PlayerHead.transform.eulerAngles = new Vector3(PlayerHead.transform.eulerAngles.x, PlayerHead.transform.eulerAngles.y, 0);
    }
}
