using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool isEnabled = true;

    // States
    bool _isGrounded = true;
    public bool IsGrounded
    {
        private set { _isGrounded = value; }
        get => _isGrounded;
    }

    bool _isFalling = false;
    public bool IsFalling
    {
        private set { _isFalling = value; }
        get => _isFalling;
    }

    bool _isRunning = false;
    public bool IsRunning
    {
        private set { _isRunning = value; }
        get => _isRunning;
    }

    private bool _isIdling = true;
    public bool IsIdling
    {
        private set { _isIdling = value; }
        get => _isIdling;
    }
    private bool _isJumping = false;
    public bool IsJumping
    {
        private set { _isJumping = value; }
        get => _isJumping;
    }
    // private bool doJump = false; //This is required for the double-jumping animation to work properly

    private bool _wasGrounded = true;
    private int _groundLayers;
    readonly float terminalFallSpeed = -10.0f;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int maxJumps;
    [HideInInspector] public float jumpHeight;
    [HideInInspector] public int _playerJumps;
    [HideInInspector] public bool usedRegJump;


    private Vector3 _playerAcceleration = Vector3.zero; // Implement later
    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _playerMovement = Vector3.zero;
    private float _playerSpeed = 0.0f;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;
    private float _rotationVelocity;
    private float _targetRotation = 0.0f;
    private Camera _camera;
    private float _rotationSmoothTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _controller = GetComponent<CharacterController>();
        _groundLayers = LayerMask.GetMask("Walls", "Default");

        usedRegJump = false;
        _playerJumps = maxJumps;

        _camera.transform.forward = transform.forward;
        // animator.SetBool("is_idle", true);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 curr_val = context.ReadValue<Vector2>();
        switch (context.phase)
        {
            case InputActionPhase.Canceled:
                _playerMovement = Vector3.zero;
                _playerSpeed = 0.0f;
                IsRunning = false;
                // animator.SetBool("is_idle", IsIdling);
                break;

            default:
                _playerMovement.x = curr_val.x;
                _playerMovement.z = curr_val.y;
                _playerSpeed = moveSpeed;
                IsRunning = true;

                // animator.SetBool("is_idle", false);
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                
                if (IsGrounded) //Might be checking the wrong type of grounded...
                {
                    // doJump = true;
                    _moveVelocity.y = jumpHeight;
                    IsJumping = true;
                }
                else if (!IsGrounded && _playerJumps > 0)
                {
                    // doJump = true;
                    _moveVelocity.y = jumpHeight;
                    IsJumping = true;
                    _playerJumps--;
                }
            
                break;
        }
    }


    void ProcessGrounded() // I could honestly call this function in the getter as well, but either works
    {
        _wasGrounded = IsGrounded;
        IsGrounded = _controller.isGrounded;

        if (!IsGrounded) // Airborne
        {
            // animator.SetBool("Airborne", true);
            if (_moveVelocity.y < -0.001f) // Falling
            {
                if (IsJumping)
                {
                    IsJumping = false; // No more jumping for you
                }
                IsFalling = true;
                //animator.SetBool("is_running", false);
            }
        }
        else // On ground
        {
            // animator.SetBool("Airborne", false);
            if (!_wasGrounded) // Landed
            {
                IsFalling = false;
                IsJumping = false; //IF this causes further jank, may want to do a larger rework of how jumping works
                _playerJumps = maxJumps; // Recover all jumps on land
                // animator.SetTrigger("landing_normal");
                // SoundBank.PlaySound("Landing");
            }

            if (_moveVelocity.y < 0f && IsGrounded)
                _moveVelocity.y = -0.0001f;
        }
        //GroundLayers, QueryTriggerInteraction.Ignore
    }

    public void ProcessInput()
    {
        Vector3 modifiedInput = new();
        if (_playerMovement.x != 0) //If moving forward or backward
            /*Actually, this is supposed to be the camera rotating when the player is moving left or right, but I don't think it was implemented correctly.
             Or it is so minor that it's impossible to notice.*/
        {
            modifiedInput = Mathf.Sign(_playerMovement.x) * _camera.transform.right;
            _camera.transform.Rotate(new Vector3(0, Mathf.Sign(_playerMovement.x) * 0.05f, 0));
        }
        else
        {
            modifiedInput = Vector3.zero;
        }
        modifiedInput += Mathf.Sign(_playerMovement.z) * _camera.transform.forward;
        modifiedInput.Normalize();
        _moveVelocity.x = modifiedInput.x;
        _moveVelocity.z = modifiedInput.z;
         Debug.DrawRay(transform.position, _moveVelocity* 10.0f, Color.red);
    }

    public void ProcessMove()
    {
        if (!isEnabled)
            return;

        ProcessInput();

        if (_playerMovement.magnitude >= 0.1f)
        {
            _targetRotation = (Mathf.Atan2(_playerMovement.x, _playerMovement.z) * Mathf.Rad2Deg +
                                        _camera.transform.eulerAngles.y);
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                 _rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        // Look into a solution that will make things smoother
        _moveDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward.normalized;

        Vector3 moveVelocityXZ = _moveDirection * _playerSpeed;
        //Move velocity is either the player's forward transform * dash speed or the move direction * regular speed
        //Gravity still affects the player while they are dashing
        _moveVelocity.x = moveVelocityXZ.x;
        _moveVelocity.z = moveVelocityXZ.z;
        _moveVelocity = AdjustVelocityToSlope(_moveVelocity);


        _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        _moveVelocity.y = Mathf.Max(terminalFallSpeed, _moveVelocity.y);
    

        _controller.Move(_moveVelocity * Time.deltaTime);
    }


    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 4.0f))
        {
            //Sends out a raycast that grabs the normal rotation of the ground
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Vector3 adjustedVelocity = slopeRotation * velocity;
            //If the adjusted velocity is moving down and the player isn't falling, return the adjusted velocity
            //Otherwise, returns the original velocity
            //BASICALLY, only affects player if on level ground or if going down a slope
            if (adjustedVelocity.y < 0 && !IsFalling && !IsJumping)
            {
                // Debug.Log("Adjusted velocity direction");
                return adjustedVelocity;
            }
        }
        //Returns original velocity if the raycast doesn't hit anything
        return velocity;
    }


    void PrintStates()
    {
        Debug.Log($"Idling: {IsIdling}, Running: {IsRunning}, Falling: {IsFalling}, "
                    + $"Jumping: {IsJumping}, Grounded: {IsGrounded}");
//                    + $"Swinging: {GetComponent<PlayerCollect>().IsCollecting}");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessGrounded();
        // animator.ResetTrigger("do_jump");
        IsIdling = (!IsFalling && !IsRunning && !IsJumping);
        // animator.SetBool("is_idle", IsIdling);
        // animator.SetBool("is_running", IsRunning);
        // animator.SetBool("is_falling", IsFalling); //Verified that this is true while y-velocity is negative and while not grounded
        // animator.SetBool("is_jumping", IsJumping);
        //Do we want jump animation to continue as long as the player is going upwards? Or only play for the duration of the jump?

        ProcessMove();

        /*Needed for double jump to work. Basically makes sure that the do_jump trigger is set AFTER it is reset each tick
         So if do_jump is set, it will be reset on the next tick to make sure it does not persist while doing other animations
        like harvesting or dashing*/
        // if (doJump)
        // {
        //     animator.SetTrigger("do_jump");
        //     doJump = false;
        //     SoundBank.PlaySound("Jump");
        // }
    }
}
