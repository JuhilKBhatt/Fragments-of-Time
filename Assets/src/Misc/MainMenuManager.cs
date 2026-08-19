using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    bool loadingLevel = false;
     void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 61;
    }

    public void loadTrailer()
    {
        if (!loadingLevel)
        {
            loadingLevel = true;
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void LoadLevel(int levelValue)
    {
        if (!loadingLevel)
        {
            loadingLevel = true;
            // Load the ManagementScene
            switch (levelValue)
            {
                case (0):
                    levelLoader.AssignLevelToLoad(level.Tutorial);
                    break;
                case (1):
                    levelLoader.AssignLevelToLoad(level.Level1);
                    break;
                case (2):
                    levelLoader.AssignLevelToLoad(level.Level2);
                    break;
                case (3):
                    levelLoader.AssignLevelToLoad(level.Level3);
                    break;
                case (4):
                    levelLoader.AssignLevelToLoad(level.Level4);
                    break;
            }
            // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadSceneAsync("ManagementScene", LoadSceneMode.Single);
        }
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}