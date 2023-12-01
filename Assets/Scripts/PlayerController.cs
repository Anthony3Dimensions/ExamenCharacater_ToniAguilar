using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController _characterController;
    private float _horizontal;
    private float _vertical;
    private Transform _camera;
    private float _gravity = -9.81f;
    private bool _isGrounded;
    private Vector3 _playerGravity;
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] private float _jumpHeight = 1;
    [SerializeField] private LayerMask _groundLayer;
    Animator _animator;

    //Victor ten piedad porfa


    // Start is called before the first frame update

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _animator = GetComponent<Animator>();


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical"); 
        Jump();
        Movement();  

    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
        _animator.SetBool("isJumping", !_isGrounded);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
        }
        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        _characterController.Move(_playerGravity * Time.deltaTime);

    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0,_vertical);
        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);



        }

    }
}
