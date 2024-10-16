using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform player;
    new public CinemachineVirtualCamera camera;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;

    public Vector3 standingUpCenter;
    public Vector3 crouchedCenter;
    public float standingUpHeight;
    public float crouchedHeight;
    public float heightLerpSpeed;

    public float standingUpCameraHeight;
    public float crouchedCameraHeight;

    public float runFOV;
    public float regularFOV;
    public float FOVLerpSpeed;

    public float verticalMouseSensitivity;
    public float horizontalMouseSensitivity;
    public float gravity;
    public InputActions actions;

    public Transform headCheck;
    public float headCheckRadius;
    public LayerMask nonPlayerLayers;

    public EventObject madeQuietNoise;
    public EventObject madeMediumNoise;

    private Vector2 m_movementDirection;
    private Vector2 m_mouseDelta;
    private float m_gravityForce;
    private float m_cameraPitch;
    private bool m_running;
    private bool m_crouching;

    private float m_moveSpeed;
    private float m_currentCameraHeight;

    void Start()
    {
        m_currentCameraHeight = standingUpCameraHeight;
        m_moveSpeed = walkSpeed;

        actions = new InputActions();
        actions.Enable();

        actions.Default.Movement.performed += ctx => 
        {
            m_movementDirection = ctx.ReadValue<Vector2>().normalized;
        };
        actions.Default.Look.performed += ctx => m_mouseDelta = ctx.ReadValue<Vector2>();

        actions.Default.Run.performed += ctx => 
        {
            if (TryUnCrouch())
            {
                m_running = true;
            } 
        };
        actions.Default.Run.canceled += ctx => m_running = false;

        actions.Default.Crouch.performed += ctx =>
        {
            if (m_crouching)
            {
                Debug.Log(TryUnCrouch());
                return;
            }

            m_running = false;
            Crouch();
        };
    }
    private void OnDisable() => actions.Disable();


    private void Crouch()
    {
        m_crouching = true;

        controller.center = crouchedCenter;
        controller.height = crouchedHeight;

        m_currentCameraHeight = crouchedCameraHeight; 
    }

    private bool TryUnCrouch()
    {
        if(Physics.CheckSphere(headCheck.position, headCheckRadius, nonPlayerLayers))
        {
            return false;
        }
        else
        {
            m_crouching = false;
            controller.center = standingUpCenter;
            controller.height = standingUpHeight;

            m_currentCameraHeight = standingUpCameraHeight;
            return true;
        }
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 moveInput = new Vector3(m_movementDirection.x, 0, m_movementDirection.y);
        float desiredFOV = m_running ? runFOV : regularFOV;

        camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, desiredFOV, Time.deltaTime * FOVLerpSpeed);

        m_gravityForce += !controller.isGrounded ? -gravity * Time.deltaTime : 0;
        moveInput.y += m_gravityForce;

        if (m_running)
        {
            m_moveSpeed = runSpeed;
            madeMediumNoise.Invoke(true);
        }
        else if (m_crouching)
        {
            m_moveSpeed = crouchSpeed;
        }
        else
        {
            m_moveSpeed = walkSpeed;
            madeQuietNoise.Invoke(true);
        }

        controller.Move((moveInput.x * player.right + moveInput.z * player.forward) *
            m_moveSpeed * Time.deltaTime);

        controller.Move(moveInput.y * Vector3.up * Time.deltaTime);

        Vector3 camLocalPos = camera.transform.localPosition;
        camera.transform.localPosition = new Vector3(camLocalPos.x,
            Mathf.Lerp(camera.transform.localPosition.y, m_currentCameraHeight, Time.deltaTime * heightLerpSpeed),
            camLocalPos.z);

        m_cameraPitch -= m_mouseDelta.y * Time.deltaTime * verticalMouseSensitivity;
        m_cameraPitch = Mathf.Clamp(m_cameraPitch, -90.0f, 90.0f);

        camera.transform.localEulerAngles = new Vector3(m_cameraPitch, 0.0f, 0.0f);
        player.rotation *= Quaternion.Euler(0.0f, m_mouseDelta.x * Time.deltaTime * horizontalMouseSensitivity, 0.0f);
    }
}
