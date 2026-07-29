using UnityEngine;

public class Leak : MonoBehaviour
{
    [Header("Reparación")]
    [SerializeField] private int points = 100;

    [Header("Destrucción")]
    [SerializeField] private float destroyDistance = 15f;

    private Transform player;
    private bool repaired;

    public static int TotalPoints { get; private set; }

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (
            player != null &&
            transform.position.y >
            player.position.y + destroyDistance
        )
        {
            Destroy(gameObject);
        }
    }

    public void Repair()
    {
        if (repaired)
        {
            return;
        }

        repaired = true;
        TotalPoints += points;

        Debug.Log(
            "Fuga reparada. Puntos totales: " +
            TotalPoints
        );

        Destroy(gameObject);
    }
}