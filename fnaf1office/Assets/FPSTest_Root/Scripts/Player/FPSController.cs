using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class FPSController : MonoBehaviour
{
    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder;
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float maxForce = 1;
    public float sensitivity;
    bool isSprinting;
    bool isCrouching;

    Rigidbody rb;
    Animator animator;
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        camHolder = GameObject.Find("CameraHolder");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraLockMove();
    }

    void Movement()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(moveInput.x,0,moveInput.y);
        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x,0,velocityChange.z);

         velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    void CameraLockMove()
    {
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);


    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    #endregion

}
