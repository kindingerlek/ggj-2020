using Core.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.DebugDraw;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core
{
    [SelectionBase]
    [RequireComponent(
        typeof(Rigidbody),
        typeof(PlayerInput)
    )]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private float gravity = -30f;

        [Header("MOVEMENT")]
        [SerializeField] private float deccl = 15f;
        [SerializeField] private float speed = 8f;
        [SerializeField] private float speedWhenGrabbing = 2f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashSpeedMultiplier;
        [SerializeField] private float rotationSpeed = 10f;


        // LOGIC
        private bool _isGrounded = true;
        private Vector3 _moveDirection;
        private Vector3 _velocity;

        private Grabbable grabbable;

        // COMPONENTS
        private Animator _animator;
        private PlayerInput _player;
        private PlayerControls _controls;
        private Rigidbody _rigidbody;
        private bool _isJumping;
        private bool _inDash;
        private bool _recoveringFromDash;
        private bool _requestingToGrab;


        #region Properties

        public float Gravity { get => gravity; set => gravity = value; }
        #endregion

        // Start is called before the first frame update
        private void Awake()
        {
            _controls = new PlayerControls();
            _rigidbody = GetComponent<Rigidbody>();
            _player = GetComponent<PlayerInput>();

            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if (_controls != null)
                _controls.Enable();

        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void OnDeviceLost()
        {
            Debug.Log("Device Lost");
        }

        private void OnDeviceRegained()
        {
            Debug.Log("Device Regained");
        }
        
        private void OnCollisionStay(Collision collision)
        {
            _isGrounded = true;

            if(collision.transform.tag == "Grabbable")
            {
                if (grabbable == null)
                {                    
                    if (_requestingToGrab)
                    {
                        var grabComponent = collision.transform.GetComponent<Grabbable>();
                        if (grabComponent.TryGrab(_player))
                            grabbable = grabComponent;
                    }
                }
            }
        }


        private void OnGrab(InputValue value)
        {
            _requestingToGrab = value.isPressed;
        }

        private void OnMove(InputValue value)
        {
            // Move and look inputs
            _moveDirection = ProjectInputToCamera(value.Get<Vector2>());
        }

        private void OnJump(InputValue value)
        {
            _isJumping = value.isPressed;

            if (_isJumping && _isGrounded)
                StartCoroutine(JumpCoroutine());
            else
                StopCoroutine(JumpCoroutine());
        }
        
        private void OnDash(InputValue value)
        {
            if (_inDash || _recoveringFromDash)
                return;
            StartCoroutine(DashCoroutine());
        }


        private void UpdateAnimator()
        {
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetFloat("ForwardSpeed", _moveDirection.magnitude);
        }

        private void Update()
        {
            DebugDraw.Line(transform.position, transform.position + _moveDirection).Color = Color.magenta;
            DebugDraw.Line(transform.position, transform.position + _velocity.normalized).Color = Color.cyan;

            if (grabbable != null && !_requestingToGrab)
            {
                grabbable.CancelGrab(_player);
                grabbable = null;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var xzVel = _rigidbody.velocity;
            xzVel.y = 0;

            // Manage verical speed
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0f;

            // Apply Gravity
            _velocity.y += gravity * Time.fixedDeltaTime;


            var currentSpeed = grabbable != null && !grabbable.AllPlayersConnect ?  speedWhenGrabbing : speed;

            if (!_inDash)
            {
                if (_moveDirection != Vector3.zero)
                {
                    _velocity.x = (_moveDirection * currentSpeed).x;
                    _velocity.z = (_moveDirection * currentSpeed).z;
                }
                else
                {
                    if (xzVel.magnitude >= 0)
                        _velocity -= (xzVel.normalized * deccl) * Time.fixedDeltaTime;

                }

                if (!_inDash && xzVel.magnitude >= currentSpeed)
                {
                    xzVel = xzVel.normalized * currentSpeed;
                    xzVel.y = _velocity.y;
                    _velocity = xzVel;
                }

                transform.rotation = GetRotationTowardsMovement(_moveDirection.normalized); 
            }

            UpdateAnimator();

            _isGrounded = false;
            _rigidbody.velocity = _velocity;
        }
        #region Coroutines
        private IEnumerator JumpCoroutine()
        {
            _velocity.y = 0;

            if (_isGrounded)
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Skip one update cycle
            yield return null;

            var enterTime = 0f;
            while (_isJumping && (enterTime += Time.deltaTime) <= 0.25f)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                // Skip onw update cycle
                yield return null;
            }

            // Kill coroutine
            yield break;
        }

        private IEnumerator DashCoroutine()
        {
            Debug.Log("Dash");
            _inDash = true;
            var dashTime = (dashDistance / (speed * dashSpeedMultiplier));
            float recoverTime = 3f;

            var auxVel = _velocity;
            while ((dashTime -= Time.deltaTime) > 0)
            {
                auxVel = _velocity;
                auxVel.y = 0;

                auxVel = auxVel.normalized * (speed * dashSpeedMultiplier);
                auxVel.y = _velocity.y;

                _velocity = auxVel;
                yield return null;
            }
            _inDash = false;

            _recoveringFromDash = true;
            while ((recoverTime -= Time.deltaTime) > 0)
                yield return null;
            _recoveringFromDash = false;
        }
        #endregion

        private Quaternion GetRotationTowardsMovement(Vector3 lookDirection)
        {
            if (lookDirection != Vector3.zero)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = 
                    Vector3.Slerp(transform.forward, lookDirection, 1 - Mathf.Exp(-rotationSpeed * Time.deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                return Quaternion.LookRotation(smoothedLookInputDirection, transform.up);
            }
            return transform.rotation;
        }

        private Vector3 ProjectInputToCamera(Vector2 inputAxis)
        {
            Quaternion cameraRotation = Camera.main.transform.rotation;

            if (PlayerInputManager.instance.splitScreen)
                cameraRotation = _player.camera.transform.rotation;

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(cameraRotation * Vector3.forward, transform.up).normalized;

            if (cameraPlanarDirection.sqrMagnitude == 0f)
                cameraPlanarDirection = Vector3.ProjectOnPlane(cameraRotation * Vector3.up, transform.up).normalized;

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, transform.up);
            return cameraPlanarRotation * new Vector3(inputAxis.x, 0, inputAxis.y);
        }
    }
}
