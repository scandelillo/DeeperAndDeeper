using UnityEngine;

public class PlayerCameraDeath : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private Collider2D playerCollider;

    [Header("Configuracion")]
    [SerializeField] private float edgeMargin = 0f;

    private bool isDead;

    private void Awake()
    {
        if (gameCamera == null)
        {
            gameCamera = Camera.main;
        }

        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }
    }

    private void LateUpdate()
    {
        if (isDead || gameCamera == null || playerCollider == null)
        {
            return;
        }

        CheckCameraEdges();
    }

    private void CheckCameraEdges()
    {
        float cameraHalfHeight = gameCamera.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * gameCamera.aspect;

        Vector3 cameraPosition = gameCamera.transform.position;

        float leftEdge =
            cameraPosition.x - cameraHalfWidth + edgeMargin;

        float rightEdge =
            cameraPosition.x + cameraHalfWidth - edgeMargin;

        float bottomEdge =
            cameraPosition.y - cameraHalfHeight + edgeMargin;

        float topEdge =
            cameraPosition.y + cameraHalfHeight - edgeMargin;

        Bounds playerBounds = playerCollider.bounds;

        bool touchedLeft =
            playerBounds.min.x <= leftEdge;

        bool touchedRight =
            playerBounds.max.x >= rightEdge;

        bool touchedBottom =
            playerBounds.min.y <= bottomEdge;

        bool touchedTop =
            playerBounds.max.y >= topEdge;

        if (touchedLeft || touchedRight ||
            touchedBottom || touchedTop)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.GameOver(
                Leak.TotalPoints
            );
        }
        else
        {
            Debug.LogError(
                "No se detecta ka escena"
            );
        }
    }
}