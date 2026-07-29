using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int pointsLost = 50;

    private bool alreadyHit;

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

            Debug.Log(
                "Tocaste un enemigo. Pierdes " +
                pointsLost +
                " puntos."
            );

            Destroy(gameObject);
        }
    }
}