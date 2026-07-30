using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int pointsLost = 50;

    private CameraFall cameraFall;
    private bool alreadyHit;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraFall =
                Camera.main.GetComponent<CameraFall>();
        }
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

            // Resta los puntos
            Leak.ChangePoints(-pointsLost);

            // Activa el shake de cámara
            if (cameraFall != null)
            {
                cameraFall.Shake();
            }

            Debug.Log(
                "Tocaste un enemigo" +
                pointsLost +
                " puntos "
            );

            Destroy(gameObject);
        }
    }
}