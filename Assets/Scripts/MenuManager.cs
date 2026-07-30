using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;

    [Header("Tutorial Panel")]
    [SerializeField] private GameObject tutorialPanel;

    [Header("Game Scene Name")]
    [SerializeField] private string gameSceneName;

    [Header("Game Scene Name")]
    [SerializeField] private AudioClip touchSound;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        AudioManager.Instance.SFX.Play(touchSound);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        AudioManager.Instance.SFX.Play(touchSound);
    }
}