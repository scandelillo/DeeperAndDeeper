using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraFall : MonoBehaviour
{
    [Header("Gravedad")]
    [SerializeField] private float initialGravityScale = 0.01f;
    [SerializeField] private float gravityIncreaseRate = 0.001f; // cuánto sube por segundo
    [SerializeField] private float maxGravityScale = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = initialGravityScale;
    }

    private void FixedUpdate()
    {
        if (rb.gravityScale < maxGravityScale)
        {
            rb.gravityScale += gravityIncreaseRate * Time.fixedDeltaTime;
            rb.gravityScale = Mathf.Min(rb.gravityScale, maxGravityScale);
        }
    }
}