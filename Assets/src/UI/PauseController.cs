using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    private bool isPaused = false;
    

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0 && !isPaused)
        {
            return;
        }
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            PauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PauseUI.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
    }
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}