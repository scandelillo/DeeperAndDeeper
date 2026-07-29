using UnityEngine;

public class Leak : MonoBehaviour
{
    [SerializeField] private int points = 100;

    public static int TotalPoints { get; private set; }

    public void Repair()
    {
        ChangePoints(points);
        Destroy(gameObject);
    }

    public static void ChangePoints(int amount)
    {
        // Evita que el puntaje baje de cero
        TotalPoints = Mathf.Max(0, TotalPoints + amount);

        Debug.Log("Puntos totales: " + TotalPoints);
    }
}