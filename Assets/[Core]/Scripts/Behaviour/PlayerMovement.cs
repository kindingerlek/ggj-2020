using Core.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core
{
    [SelectionBase]
    [RequireComponent(
        typeof(CharacterController),
        typeof(PlayerInput)
    )]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private float gravity = -30f;

        [Header("MOVEMENT")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        private const float RELEASE_GROUND_GRACETIME = 0.20f;

        // LOGIC
        private bool _isGrounded = true;
        private Vector3 _moveDirection;
        private Vector3 _velocity;
        private float _releaseGroundTime;

        // COMPONENTS
        private Animator _animator;
        private PlayerInput _player;
        private PlayerControls _controls;
        private CharacterController _character;
        private bool _isJumping;


        #region Properties

        public float Gravity { get => gravity; set => gravity = value; }
        #endregion

        // Start is called before the first frame update
        private void Awake()
        {
            _controls = new PlayerControls();
            _character = GetComponent<CharacterController>();
            _player = GetComponent<PlayerInput>();

            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if(_controls != null)
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

        private void OnMove(InputValue value)
        {
            // Move and look inputs
            _moveDirection = ProjectInputToCamera(value.Get<Vector2>());

            _velocity.x = (_moveDirection * speed).x;
            _velocity.z = (_moveDirection * speed).z;
        }

        private void OnJump(InputValue value)
        {
            _isJumping = value.isPressed;

            if (_isJumping)
                StartCoroutine(JumpCoroutine());
            else
                StopCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            _velocity.y = 0;
            _releaseGroundTime = -1f;

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

        private void OnDash(InputValue value)
        {            
            Debug.Log("Dash");
        }

        private void UpdateAnimator()
        {
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetFloat("ForwardSpeed", _moveDirection.magnitude);
        }

        // Update is called once per frame
        private void Update()
        {
            // Manage verical speed
            if (_character.isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
                _releaseGroundTime = RELEASE_GROUND_GRACETIME;
            }

            // Apply Gravity
            _velocity.y += gravity * Time.deltaTime;
            
            // Move and Rotate the character
            _character.Move(_velocity * Time.deltaTime);
            transform.rotation = GetRotationTowardsMovement(_moveDirection.normalized);

            UpdateAnimator();

            _isGrounded = _character.isGrounded || ((_releaseGroundTime -= Time.deltaTime) > 0);
        }

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
