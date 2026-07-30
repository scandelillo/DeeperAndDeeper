using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D body;

    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;


    [Header("Salto")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.15f;


    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJumpPerformed;
        controls.Player.Disable();
    }


    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isGrounded)
            jumpRequested = true;
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void Start()
    {
        
    }

 

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(moveInput.x * moveSpeed, body.linearVelocity.y);

        if (jumpRequested)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}
