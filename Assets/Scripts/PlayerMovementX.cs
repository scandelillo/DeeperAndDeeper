using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementX : MonoBehaviour
{

    [SerializeField] private AudioClip jumpSound;

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

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;

    private float dashDirection = 1f;
    private float lastDirection = 1f;

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

        // Salto
        controls.Player.Jump.performed += OnJumpPerformed;

        // Reparar fugas
        controls.Player.Interact.performed += OnInteractPerformed;

        // Dash usando la acción Sprint, normalmente asignada a Shift
        controls.Player.Sprint.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJumpPerformed;
        controls.Player.Interact.performed -= OnInteractPerformed;
        controls.Player.Sprint.performed -= OnDashPerformed;

        controls.Player.Disable();
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        // Guarda la última dirección horizontal utilizada
        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            lastDirection = Mathf.Sign(moveInput.x);
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        UpdateDashTimers();
    }

    private void FixedUpdate()
    {
        float horizontalVelocity;

        if (isDashing)
        {
            horizontalVelocity = dashDirection * dashSpeed;
        }
        else
        {
            horizontalVelocity = moveInput.x * moveSpeed;
        }

        // Solo cambia la velocidad horizontal.
        // Conserva la velocidad vertical.
        body.linearVelocity = new Vector2(
            horizontalVelocity,
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

    private void UpdateDashTimers()
    {
        if (dashCooldownRemaining > 0f)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;

            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
            }
        }
    }

    private void OnJumpPerformed(
        InputAction.CallbackContext context
    )
    {
        if (isGrounded)
        {
            jumpRequested = true;
            AudioManager.Instance.SFX.Play(jumpSound);
        }
    }

    private void OnDashPerformed(
        InputAction.CallbackContext context
    )
    {
        // No permite usar el dash mientras está en cooldown
        if (dashCooldownRemaining > 0f || isDashing)
        {
            return;
        }

        // Lee la dirección que se está presionando
        float horizontalInput =
            controls.Player.Move.ReadValue<Vector2>().x;

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            dashDirection = Mathf.Sign(horizontalInput);
            lastDirection = dashDirection;
        }
        else
        {
            // Si no hay dirección, usa la última
            dashDirection = lastDirection;
        }

        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }

    private void OnInteractPerformed(
        InputAction.CallbackContext context
    )
    {
        if (nearbyLeak == null)
        {
            Debug.Log("No hay ninguna fuga cerca.");
            return;
        }

        nearbyLeak.Repair();
        nearbyLeak = null;
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Leak leak = collision.GetComponentInParent<Leak>();

        if (leak != null && leak == nearbyLeak)
        {
            nearbyLeak = null;
        }
    }
}