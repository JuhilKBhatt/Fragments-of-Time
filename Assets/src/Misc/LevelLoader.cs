
using System.ComponentModel;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class LevelLoader : MonoBehaviour
{
    private level ChosenLevel;
    [SerializeField]
    public LevelObject TutorialConfig, Level1Config, Level2Config, Level3Config, Level4Config, playbleMenuConfig;
    private LevelObject levelObjectToLoad;
    public bool destroyObject = false;
    public int currentLevel = 0;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("levelSelector");
        if(objs.Length > 1)
        {
            if(objs[0] == this && destroyObject == false)
            {
                Destroy(objs[1].gameObject);
            }
            else
            {
                Destroy(objs[0].gameObject);
            }
        }
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ManagementScene")
        {
            Debug.Log("we in management scene");
            if (levelObjectToLoad == TutorialConfig)
            {
                if (currentLevel <= 4)
                {
                    FindAnyObjectByType<PrefabSceneManager>().InitLevel(levelObjectToLoad, false);
                    if (currentLevel > 0)
                    {
                        FindAnyObjectByType<PrefabSceneManager>().loadIntoRoom(currentLevel);
                        //FindAnyObjectByType<EchoAbility>().canNowEcho();
                    }

                }
                else
                {
                    FindAnyObjectByType<PrefabSceneManager>().InitLevel(levelObjectToLoad, true);
                    FindAnyObjectByType<PrefabSceneManager>().loadIntoRoom(currentLevel);
                    FindAnyObjectByType<EchoAbility>().canNowEcho();
                }

            }
            else
            {
                FindAnyObjectByType<PrefabSceneManager>().InitLevel(levelObjectToLoad, true);
                Debug.Log("This be the current level" + currentLevel);
                if (currentLevel > 0)
                {
                    FindAnyObjectByType<PrefabSceneManager>().loadIntoRoom(currentLevel);
                }
                FindAnyObjectByType<EchoAbility>().canNowEcho();
            }
            FindAnyObjectByType<WinController>().levelLoader = this;
            loadMechanics();
            //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.sceneLoaded -= onSceneLoaded;

            SceneManager.sceneUnloaded += unloadScene;
            //Destroy(this.gameObject);
            destroyObject = true;
        }
        if (scene.name == "MainMenuScene" && destroyObject == true)
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
            Destroy(this.gameObject);
        }
        else if (scene.name == "TrailerScene")
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }
       

    }

    private void loadMechanics()
    {
        switch (ChosenLevel)
        {
            case (level.Tutorial):
                break;
            case (level.Level1):
                break;
            case (level.Level2):
                break;
            case (level.Level3):
                break;
            case (level.Level4):
                FindAnyObjectByType<Level4Resetting>().enabled = true;
                if (currentLevel > 0)
                {
                    FindAnyObjectByType<Level4Resetting>().shouldReset = true;
                }
                break;
            case (level.PlayableMenu):
                break;

        }

    }
    void unloadScene(Scene scene)
    {
        if (scene.name == "ManagementScene")
        {
            SceneManager.sceneLoaded += onSceneLoaded;
            SceneManager.sceneUnloaded -= unloadScene;
        }
        else if (scene.name == "TrailerScene")
        {
            SceneManager.sceneLoaded += onSceneLoaded;
            SceneManager.sceneUnloaded -= unloadScene;

        }

    }
    public void moveToNextLevel()
    {
        currentLevel = 0;
        switch (ChosenLevel)
        {
            case (level.Tutorial):
                levelObjectToLoad = Level1Config;
                ChosenLevel = level.Level1;
                if (PlayerPrefs.GetInt("LevelsBeat") < 1)
                {
                    PlayerPrefs.SetInt("LevelsBeat", 1);
                }
                break;
            case (level.Level1):
                levelObjectToLoad = Level2Config;
                ChosenLevel = level.Level2; //!! remember to change this back tro level2 when we add level 2 into the game
                if (PlayerPrefs.GetInt("LevelsBeat") < 2)
                {
                    PlayerPrefs.SetInt("LevelsBeat", 2);
                }
                break;
            case (level.Level2):
                levelObjectToLoad = Level3Config;
                ChosenLevel = level.Level3;
                if (PlayerPrefs.GetInt("LevelsBeat") < 3)
                {
                    PlayerPrefs.SetInt("LevelsBeat", 3);
                }
                break;
            case (level.Level3):
                levelObjectToLoad = Level4Config;
                ChosenLevel = level.Level4;
                if (PlayerPrefs.GetInt("LevelsBeat") < 4)
                {
                    PlayerPrefs.SetInt("LevelsBeat", 4);
                }
                break;
            case (level.Level4):
                levelObjectToLoad = TutorialConfig;
                ChosenLevel = level.Tutorial; //!! do something different here, this just makes it simple
                if (PlayerPrefs.GetInt("LevelsBeat") < 5)
                {
                    PlayerPrefs.SetInt("LevelsBeat", 5);
                }
                break;

        }
        ChosenLevel = level.PlayableMenu;
        levelObjectToLoad = playbleMenuConfig;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }
    public level getCurrentLevel()
    {
        return ChosenLevel;
    }



    

    public LevelLoader getInstance()
    {
        return this;
    }

   public void AssignLevelToLoad(level newLevel)
    {
        currentLevel = 0;
        ChosenLevel = newLevel;
        switch (newLevel)
        {
            case (level.Tutorial):
                levelObjectToLoad = TutorialConfig;
                break;
            case (level.Level2):
                levelObjectToLoad = Level2Config;
                break;
            case (level.Level3):
                levelObjectToLoad = Level3Config;
                break;
            case (level.Level4):
                levelObjectToLoad = Level4Config;
                break;
            case (level.Level1):
                levelObjectToLoad = Level1Config;
                break;
            case (level.PlayableMenu):
                levelObjectToLoad = playbleMenuConfig;
                break;

        }
    }

    
}

public enum level
{
    Tutorial,
    Level1,
    Level2,
    Level3,
    Level4,
    PlayableMenu
}
