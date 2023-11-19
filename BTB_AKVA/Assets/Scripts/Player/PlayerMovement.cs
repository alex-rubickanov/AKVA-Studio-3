using System;
using System.Timers;
using UnityEngine;

namespace AKVA.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSmoothTime;
        [SerializeField] private float gravity;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float walkSpeed;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius;
        
        private CharacterController controller;
        private Vector3 playerInput;
        private Vector3 currentMoveVelocity;
        private Vector3 moveDampVelocity;

        private Vector3 currentForceVelocity;

        private const string PLATFORM_TAG = "MovingPlatform";

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            CheckMovingPlatform();

            HandleMovement();
        }

 

        private void HandleMovement()
        {
            playerInput = new Vector3
            {
                x = GetAxis(PlayerInput.Instance.Controls.left, PlayerInput.Instance.Controls.right),
                y = 0f,
                z = GetAxis(PlayerInput.Instance.Controls.backwards, PlayerInput.Instance.Controls.forward)
            };

            if (playerInput.magnitude > 1f)
            {
                playerInput.Normalize();
            }

            Vector3 moveVector = transform.TransformDirection(playerInput);
        
            // Check if shift (CAN BE ADDED HERE)

            currentMoveVelocity = Vector3.SmoothDamp(
                currentMoveVelocity, 
                moveVector * walkSpeed, 
                ref moveDampVelocity,
                moveSmoothTime
            );
            controller.enabled = true;
            controller.SimpleMove(currentMoveVelocity);
            controller.enabled = false;
        }

        private void HandleJumpAndGravity()
        {
            if (IsGrounded())
            {
                currentForceVelocity.y = -2f;
                if (Input.GetKeyDown(PlayerInput.Instance.Controls.jump))
                {
                    currentForceVelocity.y = jumpHeight;
                }
            }
            else
            {
                currentForceVelocity.y += gravity * Time.deltaTime;
            }

            controller.Move(currentForceVelocity * Time.deltaTime);
        }
        
        private float GetAxis(KeyCode negative, KeyCode positive)
        {
            if (Input.GetKey(negative) && Input.GetKey(positive))
            {
                return 0;
            }
        
            if (Input.GetKey(negative))
            {
                return -1f;
            }

            if (Input.GetKey(positive))
            {
                return 1f;
            }

            return 0;
        }

        private void CheckMovingPlatform()
        {
            if (Physics.Raycast(groundCheck.position, -groundCheck.up, out RaycastHit hit,groundCheckRadius))
            {
                if (hit.transform.CompareTag(PLATFORM_TAG))
                {
                    transform.parent = hit.transform;
                }
                else
                {
                    transform.parent = null;
                }
            }
            else
            {
                transform.parent = null;
            }
        }
       

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundCheckRadius);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
