using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Button ContinueButton;
    public LevelLoader levelLoader;
    bool loadingLevel = false;
    public GameObject areYouSurePanel;
    public bool displayLevels = false;
    public GameObject[] baseDisplays;
    public GameObject[] levelDisplays;
    void Start()
    {
        if (PlayerPrefs.GetInt("HasMadeGame") == 1)
        {

        }
        else
        {
            ContinueButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            displayLevels = !displayLevels;
            changeDisplay();
        }

    }
    void changeDisplay()
    {
        if (displayLevels)
        {
            foreach (GameObject obj in levelDisplays)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in baseDisplays)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in levelDisplays)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in baseDisplays)
            {
                obj.SetActive(true);
            }
        }

    }

    public void newGame()
    {
        //load tutorial scene
        //reset playerPreferences
        if (!loadingLevel)
        {
            loadingLevel = true;
            areYouSurePanel.SetActive(true);
        }


    }
    public void Continue()
    {
        //load the playable level select screen
        if (!loadingLevel)
        {
            loadingLevel = true;
            levelLoader.AssignLevelToLoad(level.PlayableMenu);
            SceneManager.LoadSceneAsync("ManagementScene", LoadSceneMode.Single);
        }
    }
    public void AreYouSure()
    {
        loadingLevel = false;
        areYouSurePanel.SetActive(false);

    }

    public void PrettySureThreATrashBagIntoSpaceAtWork()
    {
        loadingLevel = true;
        Debug.Log("TRYING TO LOAD NEW LEVEL");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HasMadeGame", 1);
        PlayerPrefs.SetInt("LevelsBeat", 0);
        levelLoader.AssignLevelToLoad(level.Tutorial);
        SceneManager.LoadSceneAsync("ManagementScene", LoadSceneMode.Single);

    }

}
