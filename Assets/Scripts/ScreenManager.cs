using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameOverPanel gameOverPanelScript;
    [SerializeField] private AudioClip touchSound;

    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        panelPause.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        panelPause.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void GameOver(int finalScore)
    {
        isGameOver = true;
        gameOverPanelScript.Show(finalScore);
        Time.timeScale = 0f;
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void Restart()
    {
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void BackToMenu()
    {
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI_MainMenu");
        AudioManager.Instance.SFX.Play(touchSound);
    }
}