using UnityEngine;

public class CameraFall : MonoBehaviour
{
    [Header("Velocidad de caída")]
    [SerializeField] private float baseFallSpeed = 1f;
    [SerializeField] private float speedPerPoint = 0.01f;
    [SerializeField] private float maxFallSpeed = 10f;

    [Header("Frecuencia de recálculo")]
    [SerializeField] private float updateInterval = 3f;

    private float currentFallSpeed;
    private float timer;

    private void Start()
    {
        currentFallSpeed = baseFallSpeed;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            RecalculateSpeed();
        }

        transform.position += Vector3.down * currentFallSpeed * Time.deltaTime;
    }

    private void RecalculateSpeed()
    {
        float targetSpeed = baseFallSpeed + (Leak.TotalPoints * speedPerPoint);
        currentFallSpeed = Mathf.Min(targetSpeed, maxFallSpeed);
    }
}