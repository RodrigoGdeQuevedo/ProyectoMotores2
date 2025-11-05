using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias de Componentes")]
    private CharacterController controller;
    private Animator animator;
    public Transform cameraTransform;

    [Header("Estadísticas de Movimiento")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 7.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 15.0f;

    [Header("Variables Internas")]
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float currentSpeed;

    private int inputXHash;
    private int inputYHash;
    private int isRunningHash;
    private int isGroundedHash;
    private int jumpHash;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputXHash = Animator.StringToHash("inputX");
        inputYHash = Animator.StringToHash("inputY");
        isRunningHash = Animator.StringToHash("isRunning");
        isGroundedHash = Animator.StringToHash("isGrounded");
        jumpHash = Animator.StringToHash("Jump");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        animator.SetBool(isGroundedHash, isGrounded);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 cameraForwardOnPlane = cameraTransform.forward;
        cameraForwardOnPlane.y = 0;
        cameraForwardOnPlane.Normalize();

        if (cameraForwardOnPlane != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForwardOnPlane);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camRight * horizontalInput + camForward * verticalInput).normalized;

        currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (horizontalInput == 0 && verticalInput == 0)
        {
            currentSpeed = 0;
        }

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        animator.SetFloat(inputXHash, horizontalInput, 0.1f, Time.deltaTime);
        animator.SetFloat(inputYHash, verticalInput, 0.1f, Time.deltaTime);
        animator.SetBool(isRunningHash, isRunning);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger(jumpHash);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}