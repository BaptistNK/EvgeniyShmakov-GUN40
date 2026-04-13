using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _moveVector;
    public float _speed=2f;
    public float _jumpForce = 350f;
    public bool _onGrounded;
    public Transform _groundCheck;
    public float _checkRadius = 0.5f;
    public LayerMask Ground;

    private void Start()
    {
        _rb=GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckingGround();
        Move();
        Jump();
    }

    void Move()
    {
        _moveVector.x = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(_moveVector.x * _speed, _rb.velocity.y); ;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce);
        }
    }

    void CheckingGround()
    {
        _onGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, Ground);
    }

}