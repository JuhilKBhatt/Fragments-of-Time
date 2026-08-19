using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorToLevel : MonoBehaviour
{
    private bool playerIsOverDoor;
    public SpriteRenderer inputPrompt;
    private bool fPressed;
    public bool isDoorActivated;
    public LevelLoader levelLoad;
    public int levelToLoad;
    public bool hasUnlockedNewLevel = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelLoad = FindFirstObjectByType<LevelLoader>();
        if (PlayerPrefs.GetInt("LevelsBeat") >= levelToLoad)
        {
            hasUnlockedNewLevel = true;

        }
        playerIsOverDoor = false;
        inputPrompt.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverDoor == true && Keyboard.current.fKey.isPressed && !isDoorActivated && !fPressed && hasUnlockedNewLevel)
        {
            isDoorActivated = true;
                // Load the ManagementScene
                switch (levelToLoad)
                {
                    case (0):
                        levelLoad.AssignLevelToLoad(level.Tutorial);
                        break;
                    case (1):
                        levelLoad.AssignLevelToLoad(level.Level1);
                        break;
                    case (2):
                        levelLoad.AssignLevelToLoad(level.Level2);
                        break;
                    case (3):
                        levelLoad.AssignLevelToLoad(level.Level3);
                        break;
                    case (4):
                        levelLoad.AssignLevelToLoad(level.Level4);
                        break;
                }
                SceneManager.LoadSceneAsync("ManagementScene", LoadSceneMode.Single);
            }
        if (Keyboard.current.fKey.isPressed)
        {
            fPressed = true;
        }
        else
        {
            fPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && hasUnlockedNewLevel)
        {


            playerIsOverDoor = true;
            inputPrompt.enabled = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && hasUnlockedNewLevel)
        {
            playerIsOverDoor = false;
            inputPrompt.enabled = false;
        }
    }
}
