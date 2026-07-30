using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private int pointsLost = 50;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 2f;

    private float leftLimit;
    private float rightLimit;
    private float direction;
    private bool initialized;
    private bool alreadyHit;

    private CameraFall cameraFall;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraFall = Camera.main.GetComponent<CameraFall>();
        }
    }

    // El PlatformSpawner entrega los bordes de la plataforma
    public void Initialize(float minimumX, float maximumX)
    {
        leftLimit = minimumX;
        rightLimit = maximumX;

        // Comienza caminando aleatoriamente a izquierda o derecha
        direction = Random.value < 0.5f ? -1f : 1f;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        transform.position +=
            Vector3.right *
            direction *
            moveSpeed *
            Time.deltaTime;

        Vector3 position = transform.position;

        // Llegó al borde derecho
        if (position.x >= rightLimit)
        {
            position.x = rightLimit;
            direction = -1f;
        }

        // Llegó al borde izquierdo
        else if (position.x <= leftLimit)
        {
            position.x = leftLimit;
            direction = 1f;
        }

        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyHit)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            alreadyHit = true;

            Leak.ChangePoints(-pointsLost);

            if (cameraFall != null)
            {
                cameraFall.Shake();
            }

            Debug.Log(
                "Tocaste un enemigo. Pierdes " +
                pointsLost +
                " puntos."
            );

            Destroy(gameObject);
        }
    }
}