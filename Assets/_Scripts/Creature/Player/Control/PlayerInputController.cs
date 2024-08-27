using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Vector3 _moveInput;
    [SerializeField] private float _speed;

    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 nextVec = _moveInput * _speed * Time.fixedTime;
        _rigid.MovePosition(_rigid.position + nextVec);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector3>();
    }

    private void OnLook(InputValue value)
    {
        //Debug.Log("OnLook" + value.ToString());
    }

    private void OnFire(InputValue value)
    {
        //Debug.Log("OnFire" + value.ToString());
    }
}
