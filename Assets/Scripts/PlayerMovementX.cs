using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementX : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D body;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private bool jumpRequested;
    private bool isGrounded;

    // Guarda la fuga que está tocando el jugador
    private Leak nearbyLeak;

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

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        // Escucha la acción de salto
        controls.Player.Jump.performed += OnJumpPerformed;

        // Escucha la acción para reparar la fuga
        controls.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJumpPerformed;
        controls.Player.Interact.performed -= OnInteractPerformed;

        controls.Player.Disable();
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            body.linearVelocity.y
        );

        if (jumpRequested)
        {
            body.linearVelocity = new Vector2(
                body.linearVelocity.x,
                jumpForce
            );

            jumpRequested = false;
        }
    }

    private void OnJumpPerformed(
        InputAction.CallbackContext context
    )
    {
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }

    // Se ejecuta cuando se presiona la acción Interact
    private void OnInteractPerformed(
        InputAction.CallbackContext context
    )
    {
        if (nearbyLeak == null)
        {
            Debug.Log("No hay ninguna fuga cerca.");
            return;
        }

        // Llama al método Repair del script Leak
        nearbyLeak.Repair();

        // La fuga acaba de destruirse
        nearbyLeak = null;
    }

    // Detecta cuando el jugador entra al área de la fuga
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Leak leak = collision.GetComponentInParent<Leak>();

        if (leak != null)
        {
            nearbyLeak = leak;

            Debug.Log(
                "Fuga detectada. Presiona E para repararla."
            );
        }
    }

    // Detecta cuando el jugador se aleja de la fuga
    private void OnTriggerExit2D(Collider2D collision)
    {
        Leak leak = collision.GetComponentInParent<Leak>();

        if (leak != null && leak == nearbyLeak)
        {
            nearbyLeak = null;
        }
    }
}