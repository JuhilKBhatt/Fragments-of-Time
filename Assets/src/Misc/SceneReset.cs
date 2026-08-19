using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    [SerializeField] private KeyCode resetKey = KeyCode.Backspace;

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ReloadCurrentScene();
        }
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}