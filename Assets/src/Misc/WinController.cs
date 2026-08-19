using UnityEngine;

public class WinController : MonoBehaviour
{
    public GameObject winUI;
    public GameObject levelWinUI;
    public LevelLoader levelLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win()
    {
      /*  if(levelLoader.getCurrentLevel() == level.Level4)
        {
            levelWinUI.SetActive(true);
            
        }
        else
        {*/
        winUI.SetActive(true);
      //  }
        Time.timeScale = 0;
    }

    public void ToNextLevel()
    {
        levelLoader.moveToNextLevel();
        Time.timeScale = 1;
    }
}
