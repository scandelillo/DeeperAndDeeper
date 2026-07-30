using UnityEngine;

public class CameraFall : MonoBehaviour
{
    [Header("Velocidad de caída")]
    [SerializeField] private float baseFallSpeed = 1f;
    [SerializeField] private float speedPerPoint = 0.01f;
    [SerializeField] private float maxFallSpeed = 10f;

    [Header("Frecuencia de recálculo")]
    [SerializeField] private float updateInterval = 3f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 0.25f;

    private float currentFallSpeed;
    private float timer;

    private float shakeTimeRemaining;
    private Vector3 currentShakeOffset;

    private void Start()
    {
        currentFallSpeed = baseFallSpeed;
    }

    private void Update()
    {
        // Esto hace que la camara no se desvie
        transform.position -= currentShakeOffset;
        currentShakeOffset = Vector3.zero;

        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            RecalculateSpeed();
        }

        // Movimiento normal hacia abajo
        transform.position +=
            Vector3.down *
            currentFallSpeed *
            Time.deltaTime;

        // Aplica el shake
        if (shakeTimeRemaining > 0f)
        {
            shakeTimeRemaining -= Time.deltaTime;

            Vector2 randomOffset =
                Random.insideUnitCircle * shakeStrength;

            currentShakeOffset = new Vector3(
                randomOffset.x,
                randomOffset.y,
                0f
            );

            transform.position += currentShakeOffset;
        }
    }

    private void RecalculateSpeed()
    {
        float targetSpeed =
            baseFallSpeed +
            (Leak.TotalPoints * speedPerPoint);

        currentFallSpeed = Mathf.Min(
            targetSpeed,
            maxFallSpeed
        );
    }

    public void Shake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}