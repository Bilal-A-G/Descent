using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform player;
    new public Transform camera;
    public float walkSpeed;
    public float runSpeed;
    public float verticalMouseSensitivity;
    public float horizontalMouseSensitivity;
    public float gravity;
    public InputActions actions;
    public float jumpForce;
    public float jumpBuffering;

    public EventObject madeQuietNoise;
    public EventObject madeMediumNoise;

    private Vector2 m_movementDirection;
    private Vector2 m_mouseDelta;
    private float m_gravityForce;
    private float m_lastJumpInput;
    private float m_lastJumpTime;
    private float m_cameraPitch;
    private bool m_running;

    private float m_moveSpeed;

    void Start()
    {
        m_moveSpeed = walkSpeed;

        actions = new InputActions();
        actions.Enable();

        actions.Default.Movement.performed += ctx => 
        {
            m_movementDirection = ctx.ReadValue<Vector2>().normalized;
        };
        actions.Default.Look.performed += ctx => m_mouseDelta = ctx.ReadValue<Vector2>();
        actions.Default.Jump.performed += ctx =>
        {
            if (controller.isGrounded)
            {
                m_gravityForce = 0f;
                m_gravityForce += jumpForce;
            }
            else
            {
                m_lastJumpInput = jumpForce;
            }

            m_lastJumpTime = Time.time;
            madeMediumNoise.Invoke(true);
        };
        actions.Default.Run.performed += ctx => m_running = true;
        actions.Default.Run.canceled += ctx => m_running = false;
    }
    private void OnDisable() => actions.Disable();

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 moveInput = new Vector3(m_movementDirection.x, 0, m_movementDirection.y);

        if (controller.isGrounded)
        {
            m_gravityForce = 0.0f;
            if (Time.time - m_lastJumpTime > jumpBuffering)
                m_lastJumpInput = 0.0f;
            else
                m_gravityForce += m_lastJumpInput;
        }

        m_gravityForce += !controller.isGrounded ? -gravity * Time.deltaTime : 0;
        moveInput.y += m_gravityForce;

        if (m_running)
        {
            m_moveSpeed = runSpeed;
            madeMediumNoise.Invoke(true);
        }
        else
        {
            m_moveSpeed = walkSpeed;
            madeQuietNoise.Invoke(true);
        }

        controller.Move((moveInput.x * player.right + moveInput.z * player.forward) *
            m_moveSpeed * Time.deltaTime);

        controller.Move(moveInput.y * Vector3.up * Time.deltaTime);


        m_cameraPitch -= m_mouseDelta.y * Time.deltaTime * verticalMouseSensitivity;
        m_cameraPitch = Mathf.Clamp(m_cameraPitch, -90.0f, 90.0f);

        camera.localEulerAngles = new Vector3(m_cameraPitch, 0.0f, 0.0f);
        player.rotation *= Quaternion.Euler(0.0f, m_mouseDelta.x * Time.deltaTime * horizontalMouseSensitivity, 0.0f);
    }
}
