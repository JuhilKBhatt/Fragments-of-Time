
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ManageScenes : MonoBehaviour
{/*
    public float test;
    [SerializeField]
    SceneObject pastScene;
    //string pastSceneName;
    [SerializeField]
    SceneObject futureScene;
    //string futureSceneName;
    public SceneType currentScene;
    private GameObject pastTileMap;
    private GameObject futureTileMap;
    private bool canTimeTravel = true;
    public GameObject blockOutSymbol;

    

    void Start()
    {
        SceneManager.LoadScene(pastScene.scene.name, LoadSceneMode.Additive);
        SceneManager.LoadScene(futureScene.scene.name, LoadSceneMode.Additive);
        Invoke("GetTileMaps", 1);
        //GetTileMaps();
        currentScene = SceneType.pastScene;
        Invoke("ChangeAfterStart", 1);
    }

    public void ChangeAfterStart()
    {
        ChangeToPast();
    }
    public void GetTileMaps()
    {
        if (GameObject.FindWithTag("PastTileMap") != null)
        {
            pastTileMap = GameObject.FindWithTag("PastTileMap");
        }
        if (GameObject.FindWithTag("FutureTileMap") != null)
        {
            futureTileMap = GameObject.FindWithTag("FutureTileMap");
        }
    }
    private void ChangeToPast()
    {
        currentScene = SceneType.pastScene;
        if (futureTileMap != null)
        {
            DisableTileMapElements(futureTileMap);
            if (pastTileMap != null)
            {
                EnableTileMapElements(pastTileMap);
            }
            else
            {
                Debug.Log("Whoops, could not find tilemap");
            }
        }
    }
    private void ChangeToFuture()
    {
        currentScene = SceneType.futureScene;
        if (pastTileMap != null)
        {
            DisableTileMapElements(pastTileMap);
            if (futureTileMap != null)
            {
                EnableTileMapElements(futureTileMap);
            }
            else
            {
                Debug.Log("Whoops, could not find tilemap");
            }
        }
    }
    private void EnableTileMapElements(GameObject tilemap)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        tilemap.GetComponent<TilemapCollider2D>().isTrigger = false;
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        foreach (SpriteRenderer sprite in tempMapList)
        {
            sprite.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        Debug.Log("we have enabled " + tilemap.name);
    }
    private void DisableTileMapElements(GameObject tilemap)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        tilemap.GetComponent<TilemapCollider2D>().isTrigger = true;
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        foreach (SpriteRenderer sprite in tempMapList)
        {
            if(sprite.gameObject.tag != "ZoneTrigger")
            {
            sprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        Debug.Log("we have disabled " + tilemap.name);
    }


    public void ChangeScenes(InputAction.CallbackContext context)
    {
        if(context.started && canTimeTravel == true)
        {
            Debug.Log("here is the context, we will probs change this up so its not like this " + context);
            if(currentScene == SceneType.pastScene)
            {
                ChangeToFuture();
            }
            else if(currentScene == SceneType.futureScene)
            {
                ChangeToPast();

            }
            else
            {
                Debug.Log("No current scene type, this is a warning");
            }
            Debug.Log("Scene type is equal to " + currentScene);
        }
    }
    public void StopPlayerFromTimeTravel()
    {
        canTimeTravel = false;
        Debug.Log("Player can no longer time travel");
        blockOutSymbol.SetActive(true);

    }
    public void AllowPlayerToTimeTravel()
    {
        canTimeTravel = true;
        Debug.Log("Player can now time travel again");
        blockOutSymbol.SetActive(false);

    }
   */
}
