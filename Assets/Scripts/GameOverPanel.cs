using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void Show(int finalScore)
    {
        gameObject.SetActive(true);
        scoreText.text = "Puntaje: " + finalScore;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}