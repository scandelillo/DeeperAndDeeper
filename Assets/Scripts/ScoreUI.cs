using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        scoreText.text = "Puntos: " + Leak.TotalPoints;
    }
}