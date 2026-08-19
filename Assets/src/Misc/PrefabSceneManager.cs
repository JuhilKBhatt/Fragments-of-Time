
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using TMPro;
using JetBrains.Annotations;

public class PrefabSceneManager : MonoBehaviour
{
    public float test;
    [SerializeField]
    public LevelObject levelConfig;
    public PlayerRespawn playerRespawn;
    //string pastSceneName;
    //[SerializeField]
    //SceneObject futureScene;
    //string futureSceneName;
    private TileMapObject currentPastObject;
    private TileMapObject currentFutureObject;
    private GameObject pastMap;
    private GameObject futureMap;
    public SceneType currentScene;
    private List<GameObject> pastTileMaps = new List<GameObject>();
    private List<GameObject> futureTileMaps = new List<GameObject>();
    private bool canTimeTravel = true;
    public Sprite fixedHourGlassIcon;
    public Sprite brokenHourGlassIcon;
    public Image shiftIndicator;
    public Image shiftButtonIndicator;
    public GameObject blockOutSymbol;
    private int currentRoomNumber = 0;
    private Coroutine visibilityCoroutine;
    public Light2D e;
    public bool inputDisabled = false;

    public CinemachineCamera camera;
    private bool AbilityActivated = false;
    public ShaderInAndOut vortex;



    public void InitLevel(LevelObject chosenLevel, bool canUseAbility)
    {
        AbilityActivated = canUseAbility;
        levelConfig = null;
        levelConfig = chosenLevel;
        createAllRooms();
        if (AbilityActivated == false)
        {
            shiftIndicator.gameObject.SetActive(false);
            FindAnyObjectByType<EchoAbility>().DisableStuffAtBegin();
        }
        //SetUpThings();
        //Invoke("GetTileMaps", 1);
        //GetTileMaps();
        currentScene = SceneType.pastScene;
        ChangeAfterStart();
    }

    void SetUpThings()
    {
        currentPastObject = levelConfig.pastRooms[0];
        currentFutureObject = levelConfig.futureRooms[0];
        pastMap = Instantiate(currentPastObject.TilemapPrefab).GetComponentInChildren<Tilemap>().gameObject;
        futureMap = Instantiate(currentFutureObject.TilemapPrefab).GetComponentInChildren<Tilemap>().gameObject;
    }

    public void ChangeAfterStart()
    {
        ChangeToFuture();
        setCameraAndRoom();
        changeBeforeAndAfterTileMap();

    }
    public void GetTileMaps()
    {

        //pastTileMap = Instantiate(ps)
        /*if (GameObject.FindWithTag("PastTileMap") != null)
        {
            pastTileMap = GameObject.FindWithTag("PastTileMap");
        }
        if (GameObject.FindWithTag("FutureTileMap") != null)
        {
            futureTileMap = GameObject.FindWithTag("FutureTileMap");
        }*/
    }
    private void ChangeToPast(bool hasMoved = false)
    {
        currentScene = SceneType.pastScene;
        if (futureMap != null)
        {
            DisableTileMapElements(futureMap, hasMoved);
            if (pastMap != null)
            {
                EnableTileMapElements(pastMap, hasMoved);
            }
            else
            {
                //  Debug.Log("Whoops, could not find tilemap");
            }
        }


    }


    private void changeBeforeAndAfterTileMap()
    {
        if (currentRoomNumber == 0 && levelConfig.futureRooms.Length > 1)
        {
            if (currentScene == SceneType.futureScene)
            {
                EnableTileMapElements(futureTileMaps[1], false);
                DisableTileMapElements(pastTileMaps[1], false);
            }
            else
            {
                EnableTileMapElements(pastTileMaps[1], false);
                DisableTileMapElements(futureTileMaps[1], false);
            }

        }
        else if (currentRoomNumber == levelConfig.pastRooms.Length - 1 && levelConfig.futureRooms.Length > 1)
        {
            if (currentScene == SceneType.futureScene)
            {
                EnableTileMapElements(futureTileMaps[currentRoomNumber - 1], false);
                DisableTileMapElements(pastTileMaps[currentRoomNumber - 1], false);
            }
            else
            {
                EnableTileMapElements(pastTileMaps[currentRoomNumber - 1], false);
                DisableTileMapElements(futureTileMaps[currentRoomNumber - 1], false);
            }

        }
        else if (levelConfig.futureRooms.Length > 2)
        {
            if (currentScene == SceneType.futureScene)
            {
                EnableTileMapElements(futureTileMaps[currentRoomNumber - 1], false);
                DisableTileMapElements(pastTileMaps[currentRoomNumber - 1], false);
                EnableTileMapElements(futureTileMaps[currentRoomNumber + 1], false);
                DisableTileMapElements(pastTileMaps[currentRoomNumber + 1], false);
            }
            else
            {
                DisableTileMapElements(futureTileMaps[currentRoomNumber - 1], false);
                EnableTileMapElements(pastTileMaps[currentRoomNumber - 1], false);
                DisableTileMapElements(futureTileMaps[currentRoomNumber + 1], false);
                EnableTileMapElements(pastTileMaps[currentRoomNumber + 1], false);
            }
        }

    }
    private void ChangeToFuture(bool hasMoved = false)
    {
        currentScene = SceneType.futureScene;
        if (pastMap != null)
        {
            DisableTileMapElements(pastMap, hasMoved);
            if (futureMap != null)
            {
                EnableTileMapElements(futureMap, hasMoved);
            }
            else
            {
                //  Debug.Log("Whoops, could not find tilemap");
            }
        }
    }
    private void EnableTileMapElements(GameObject tilemap, bool MovedRooms = false)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        TilemapRenderer[] tileMaps = tilemap.GetComponentsInChildren<TilemapRenderer>();
        tilemap.GetComponent<CompositeCollider2D>().isTrigger = false;
        tilemap.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f);
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        GameObject button = null;
        GameObject door = null;
        //Debug.Log(tilemap.GetComponent<Tilemap>().cellBounds.center + " Is the center of the tilemap");
        foreach (TilemapRenderer map in tileMaps)
        {
            if (map.gameObject.CompareTag("Water"))
            {
                map.enabled = true;
                map.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
                map.gameObject.GetComponent<WaterDetect>().enabled = true;
            }
            else if (map.gameObject.CompareTag("Background"))
            {
                map.enabled = true;
            }
        }
        foreach (SpriteRenderer sprite in tempMapList)
        {
            if (sprite.gameObject.tag == "Door" || sprite.gameObject.tag == "Button" || sprite.gameObject.tag == "Lever")
            {
                if (sprite.gameObject.CompareTag("Button"))
                {
                    sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
                    button = sprite.gameObject;
                    button.GetComponentInParent<ButtonTrigger>().getInput(true);
                    button.GetComponentInParent<ButtonTrigger>().ReTriggerDoor();


                }
                if (sprite.gameObject.CompareTag("Lever"))
                {
                    sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
                    button = sprite.transform.parent.gameObject;
                    button.GetComponent<ButtonTriggerMultiple>().activateDoors(true);
                }
                if (sprite.gameObject.CompareTag("Door"))
                    {
                        door = sprite.gameObject;
                        sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
                        sprite.gameObject.GetComponent<BoxCollider2D>().enabled = true;

                    }
            }
            else if (sprite.gameObject.tag != "ZoneTrigger")
            {
                if (sprite.gameObject.CompareTag("Instructions"))
                {
                    sprite.gameObject.GetComponentInChildren<TMP_Text>().enabled = true;
                }
                if (sprite.gameObject.CompareTag("PushableBox"))
                {
                    sprite.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

                }
                else if (sprite.gameObject.CompareTag("RollableObject"))
                {
                    sprite.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    sprite.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                    sprite.gameObject.GetComponent<SaveRollingObjectVelocity>().enabled = true;
                }
                else if (sprite.gameObject.CompareTag("Ground"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                }
                else if (sprite.gameObject.CompareTag("Sign"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    sprite.gameObject.GetComponentInChildren<Light2DBase>().enabled = true;
                }
                else if (sprite.gameObject.CompareTag("Breakable"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                }
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
            }
            else
            {
                sprite.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                if (!sprite.gameObject.GetComponent<RoomTrigger>().loadNextRoom && MovedRooms)
                {
                    sprite.gameObject.GetComponent<RoomTrigger>().DontActivateYet = true;
                }
            }
        }
        //Debug.Log("we have enabled " + tilemap.name);
    }
    private void DisableTileMapElements(GameObject tilemap, bool MovedRooms = false)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        TilemapRenderer[] tileMaps = tilemap.GetComponentsInChildren<TilemapRenderer>();
        tilemap.GetComponent<CompositeCollider2D>().isTrigger = true;
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        GameObject door = null;
        foreach (TilemapRenderer map in tileMaps)
        {
            map.enabled = false;
            if (map.gameObject.CompareTag("Water"))
            {
                map.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
                map.gameObject.GetComponent<WaterDetect>().enabled = false;
            }
            else if (map.gameObject.CompareTag("Background"))
            {
                map.enabled = false;
            }
        }
        foreach (SpriteRenderer sprite in tempMapList)
        {
            if (sprite.gameObject.CompareTag("Door"))
            {
                door = sprite.gameObject;

                sprite.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (sprite.gameObject.CompareTag("Button"))
            {
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
                // sprite.gameObject.GetComponentInParent<ButtonTrigger>().ReTriggerDoor();
                sprite.gameObject.GetComponentInParent<ButtonTrigger>().getInput(false);


            }
            if (sprite.gameObject.CompareTag("Lever"))
            {
                GameObject button;
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
                button = sprite.transform.parent.gameObject;
                button.GetComponent<ButtonTriggerMultiple>().activateDoors(false);
            }
            if (sprite.gameObject.tag != "ZoneTrigger")
            {
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
                if (sprite.gameObject.CompareTag("PushableBox"))
                {
                    sprite.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                }
                else if (sprite.gameObject.CompareTag("RollableObject"))
                {
                    sprite.gameObject.GetComponent<SaveRollingObjectVelocity>().enabled = false;
                    sprite.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    sprite.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                }
                else if (sprite.gameObject.CompareTag("Ground"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                }
                else if (sprite.gameObject.CompareTag("Sign"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    sprite.gameObject.GetComponentInChildren<Light2DBase>().enabled = false;
                }
                else if (sprite.gameObject.CompareTag("Breakable"))
                {
                    sprite.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                }
                else if (sprite.gameObject.CompareTag("Instructions"))
                {
                    sprite.gameObject.GetComponentInChildren<TMP_Text>().enabled = false;
                }

            }
            else
            {
                sprite.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                if (!sprite.gameObject.GetComponent<RoomTrigger>().loadNextRoom && MovedRooms)
                {
                    sprite.gameObject.GetComponent<RoomTrigger>().DontActivateYet = true;
                }
            }
        }
        // Debug.Log("we have disabled " + tilemap.name);
    }


    public void ChangeScenes(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0 && !inputDisabled)
        {
            if (context.interaction is HoldInteraction && context.performed && AbilityActivated)
            {
                if (visibilityCoroutine != null)
                {
                    StopCoroutine(visibilityCoroutine);
                }
                if (currentScene == SceneType.pastScene)
                {
                    makePartlyVisable(futureMap);
                }
                else if (currentScene == SceneType.futureScene)
                {
                    makePartlyVisable(pastMap);

                }
            }

            if (context.interaction is TapInteraction && context.performed && canTimeTravel == true && AbilityActivated)
            {
                if (visibilityCoroutine != null)
                {
                    StopCoroutine(visibilityCoroutine);
                }
                //Debug.Log("here is the context, we will probs change this up so its not like this " + context);
                if (currentScene == SceneType.pastScene)
                {
                    ChangeToFuture();
                    currentScene = SceneType.futureScene;
                }
                else if (currentScene == SceneType.futureScene)
                {
                    ChangeToPast();
                    currentScene = SceneType.pastScene;

                }
                else
                {
                    // Debug.Log("No current scene type, this is a warning");
                }
                // Debug.Log("Scene type is equal to " + currentScene);
                vortex.ActiateShader();

                changeBeforeAndAfterTileMap();
                this.GetComponent<PlaySoundEffect>().PlaySFX(0);
                FindAnyObjectByType<PlayerMovement>().changedTimeStopPush();
                if (FindAnyObjectByType<echoDetector>() != null)
                {
                    FindAnyObjectByType<echoDetector>().CheckForCollisions(currentScene);
                }
            }
            else if (context.interaction is TapInteraction && context.performed && canTimeTravel == false && AbilityActivated)
            {
                if (visibilityCoroutine != null)
                {
                    StopCoroutine(visibilityCoroutine);
                }
                if (currentScene == SceneType.pastScene)
                {
                    makePartlyVisable(futureMap);
                    visibilityCoroutine = StartCoroutine(makeInvisibleAgainEnumerator(futureMap));
                }
                else if (currentScene == SceneType.futureScene)
                {
                    makePartlyVisable(pastMap);
                    visibilityCoroutine = StartCoroutine(makeInvisibleAgainEnumerator(pastMap));


                }
                this.GetComponent<PlaySoundEffect>().PlaySFX(1);

            }
            else if (context.canceled && context.interaction is HoldInteraction && AbilityActivated)
            {
                if (currentScene == SceneType.pastScene)
                {
                    makeInvisibleAgain(futureMap);
                }
                else if (currentScene == SceneType.futureScene)
                {
                    makeInvisibleAgain(pastMap);

                }
            }
        }

    }
    public void StopPlayerFromTimeTravel()
    {
        canTimeTravel = false;
        // Debug.Log("Player can no longer time travel");
        //blockOutSymbol.SetActive(true);
        //shiftIndicator.sprite = brokenHourGlassIcon;
        CancelInvoke("changeHourGlassFixed");
        Invoke("changeHourGlassBroken", 0.04f);
        FindAnyObjectByType<EchoAbility>().notAbleToMakeEcho();


    
    }
    public void reEneableInput(bool icons)
    {
        inputDisabled = false;
        if (icons)
        {
            changeHourGlassFixed();
        }
    }
    public void AllowPlayerToTimeTravel()
    {
        canTimeTravel = true;
        // Debug.Log("Player can now time travel again");
        //blockOutSymbol.SetActive(false);
        CancelInvoke("changeHourGlassBroken");
        Invoke("changeHourGlassFixed", 0.04f);

        //shiftIndicator.sprite = fixedHourGlassIcon;
        FindAnyObjectByType<EchoAbility>().ableToMakeEcho();

    }
    private void changeHourGlassBroken()
    {
        if (AbilityActivated)
        {

            shiftIndicator.sprite = brokenHourGlassIcon;
            shiftButtonIndicator.enabled = false;
        }
    }
    private void changeHourGlassFixed()
    {
        if (AbilityActivated)
        {
            shiftIndicator.sprite = fixedHourGlassIcon;
            shiftButtonIndicator.enabled = true;
        }

    }
    public IEnumerator makeInvisibleAgainEnumerator(GameObject tilemap)
    {

        yield return new WaitForSeconds(1);
        makeInvisibleAgain(tilemap);
        yield return null;
    }

    public void makePartlyVisable(GameObject tilemap)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        tilemap.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.2f);
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        foreach (SpriteRenderer sprite in tempMapList)
        {

            if (sprite.gameObject.tag != "ZoneTrigger")
            {
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            }

        }
        //  Debug.Log("we have made slightly visable " + tilemap.name);

    }
    public void makeInvisibleAgain(GameObject tilemap)
    {
        SpriteRenderer[] tempMapList = tilemap.GetComponentsInChildren<SpriteRenderer>();
        tilemap.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0f);
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        foreach (SpriteRenderer sprite in tempMapList)
        {

            if (sprite.gameObject.tag != "ZoneTrigger")
            {
                sprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            }

        }
        //Debug.Log("we have made this invisible  " + tilemap.name);
    }

    public bool canWeCurrentlyTimeTravel()
    {
        return canTimeTravel;
    }

    public void MoveCamera(float XMovement)
    {
        Camera cam = Camera.main;
        cam.gameObject.transform.position += new UnityEngine.Vector3(XMovement, 0, 0);
    }

    public void loadIntoRoom(int roomNumber)
    {
        currentRoomNumber = roomNumber;
        currentPastObject = levelConfig.pastRooms[currentRoomNumber];
        currentFutureObject = levelConfig.futureRooms[currentRoomNumber];
        pastMap = pastTileMaps[currentRoomNumber];
        futureMap = futureTileMaps[currentRoomNumber];
        if (levelConfig.RoomStartsInPast.Length > roomNumber)
        {
            if (levelConfig.RoomStartsInPast[roomNumber] == true)
            {
                currentScene = SceneType.pastScene;
            }
        }
        if (currentScene == SceneType.futureScene)
        {
            ChangeToFuture(true);
        }
        else
        {
            ChangeToPast(true);
        }
        changeBeforeAndAfterTileMap();
        setCameraAndRoom();
    }

    public void GoBackOneLevel(GameObject triggerObject)
    {
        if (triggerObject.transform.IsChildOf(futureTileMaps[currentRoomNumber - 1].transform) || triggerObject.transform.IsChildOf(pastTileMaps[currentRoomNumber - 1].transform))
        {
            Debug.Log("We are going back 1 room");
            if (currentPastObject != levelConfig.pastRooms[0])
            {
                currentRoomNumber -= 1;
                FindAnyObjectByType<LevelLoader>().currentLevel -= 1;
                currentPastObject = levelConfig.pastRooms[currentRoomNumber];
                currentFutureObject = levelConfig.futureRooms[currentRoomNumber];
                pastMap = pastTileMaps[currentRoomNumber];
                futureMap = futureTileMaps[currentRoomNumber];
                if (currentScene == SceneType.futureScene)
                {
                    ChangeToFuture(true);
                }
                else
                {
                    ChangeToPast(true);
                }
                //  MoveCamera(-levelConfig.sizeOfRoomX);
            }
            else
            {
                // Debug.Log("Cannot go backwards here");
            }
        }
        else
        {
            // Debug.Log("Cannot go back here as trigger not part of current map");
        }
        FindAnyObjectByType<EchoAbility>().RemoveEcho();
        changeBeforeAndAfterTileMap();
        backCameraAndRoom();
    }
    public void GoForwardOneLevel(GameObject triggerObject)
    {
        if (triggerObject.transform.IsChildOf(futureMap.transform) || triggerObject.transform.IsChildOf(pastMap.transform))
        {
            if (currentPastObject != levelConfig.pastRooms[levelConfig.pastRooms.Length - 1])
            {
                currentRoomNumber += 1;
                FindAnyObjectByType<LevelLoader>().currentLevel += 1;
                currentPastObject = levelConfig.pastRooms[currentRoomNumber];
                currentFutureObject = levelConfig.futureRooms[currentRoomNumber];
                pastMap = pastTileMaps[currentRoomNumber];
                futureMap = futureTileMaps[currentRoomNumber];
                if (currentScene == SceneType.futureScene)
                {
                    ChangeToFuture(true);
                }
                else
                {
                    ChangeToPast(true);
                }
                //MoveCamera(levelConfig.sizeOfRoomX);
            }
            else
            {
                //  Debug.Log("Cannot go Forwards here");
            }
        }
        else
        {
            // Debug.Log("Tried to do room stuff on a object not part of this map");
        }
        if (triggerObject.GetComponent<RoomTrigger>().allowGoingBack == false)
        {
            triggerObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        FindAnyObjectByType<EchoAbility>().RemoveEcho();
        changeBeforeAndAfterTileMap();
        setCameraAndRoom();

    }

    private void setCameraAndRoom()
    {
        Vector3 centerOfMap = pastMap.GetComponent<Tilemap>().cellBounds.center;
        camera.Target.TrackingTarget = pastMap.GetComponentInChildren<centerMapScript>().gameObject.transform;
        if (pastMap.GetComponentInChildren<RespawnPoint>() != null)
        {
            playerRespawn.SetRespawn(pastMap.GetComponentInChildren<RespawnPoint>().gameObject.transform.position);
        }
    }
    private void backCameraAndRoom()
    {
        Vector3 centerOfMap = pastMap.GetComponent<Tilemap>().cellBounds.center;
        camera.Target.TrackingTarget = pastMap.GetComponentInChildren<centerMapScript>().gameObject.transform;
        if (pastMap.GetComponentInChildren<RespawnPoint>() != null)
        {
            playerRespawn.OnlySetRespawn(pastMap.GetComponentInChildren<RespawnPoint>().gameObject.transform.position);
            playerRespawn.justMovePosition(pastMap.GetComponentInChildren<BackSpawnPoint>().gameObject.transform.position);
        }
    }

    public void CanUseAbility()
    {
        AbilityActivated = true;
        shiftIndicator.gameObject.SetActive(true);

    }
    

    public void HourGlassBroken()
    {
        ChangeToPast();
        changeBeforeAndAfterTileMap();
        vortex.ActiateShader();
        canTimeTravel = true;

        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
    }
    public void epilougeTimeTravel()
    {
        if (currentScene == SceneType.futureScene)
        {
            timeTravelToPast();
        }
        else
        {
            timeTravelToFuture();
        }
    }
    private void timeTravelToFuture()
    {
        ChangeToFuture();
        changeBeforeAndAfterTileMap();
        vortex.ActiateShader();
        canTimeTravel = true;

        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
    }
    private void timeTravelToPast()
    {
        ChangeToPast();
        changeBeforeAndAfterTileMap();
        vortex.ActiateShader();
        canTimeTravel = true;

        this.GetComponent<PlaySoundEffect>().PlaySFX(0);
    }


    private void createAllRooms()
    {
        float currentXPos = 0;
        for (int i = 0; i <= levelConfig.pastRooms.Length - 1; i++)
        {
            pastTileMaps.Add(Instantiate(levelConfig.pastRooms[i].TilemapPrefab, new UnityEngine.Vector3(currentXPos, 0, 0), this.gameObject.transform.rotation).GetComponentInChildren<Tilemap>().gameObject);
            futureTileMaps.Add(Instantiate(levelConfig.futureRooms[i].TilemapPrefab, new UnityEngine.Vector3(currentXPos, 0, 0), this.gameObject.transform.rotation).GetComponentInChildren<Tilemap>().gameObject);
            currentXPos += levelConfig.sizeOfRoomX;
            //EnableTileMapElements(pastTileMaps[i]);
            if (currentScene == SceneType.pastScene)
            {
                DisableTileMapElements(futureTileMaps[i]);

            }
            else
            {
                DisableTileMapElements(pastTileMaps[i]);
            }

        }
        currentPastObject = levelConfig.pastRooms[currentRoomNumber];
        currentFutureObject = levelConfig.futureRooms[currentRoomNumber];
        pastMap = pastTileMaps[currentRoomNumber];
        futureMap = futureTileMaps[currentRoomNumber];
        // loadIntoRoom(currentRoomNumber);
    }

    public void DrainWaterFromScene()
    {
        Tilemap[] tileMaps = futureMap.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap sprite in tileMaps)
        {

            if (sprite.gameObject.tag == "Water")
            {
                Destroy(sprite.gameObject);
                //  Debug.Log("We have drained the water from this level");
            }

        }
        foreach (SpriteRenderer wall in futureMap.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if (wall.gameObject.tag == "WallToBeBroken")
            {
                Destroy(wall);
            }
        }
    }


    public void ResetCurrentLevel()
    {
        //   Debug.Log("Resetting current level...");

        // Disable both the past and future maps before resetting.
        DisableTileMapElements(pastMap);
        DisableTileMapElements(futureMap);

        // Clear the current tilemap lists.
        pastTileMaps.Clear();
        futureTileMaps.Clear();

        createAllRooms();

        // Reset the current scene to the correct scene (past or future) based on where the player currently is.
        if (currentScene == SceneType.pastScene)
        {
            ChangeToPast(); // Activate the past map
        }
        else if (currentScene == SceneType.futureScene)
        {
            ChangeToFuture(); // Activate the future map
        }
        //  setCameraAndRoom();

        // Debugging log to ensure reset is successful
        //   Debug.Log("Level reset completed. Current scene: " + currentScene);
    }

    private void DisableCurrentRooms()
    {
        // Disable or destroy the current tilemaps (if any) to clear the level
        foreach (var pastMap in pastTileMaps)
        {
            if (pastMap != null)
            {
                pastMap.SetActive(false);
            }
        }

        foreach (var futureMap in futureTileMaps)
        {
            if (futureMap != null)
            {
                futureMap.SetActive(false);
            }
        }
    }

    private void CreateNewRooms()
    {
        // Recreate the rooms based on the current level configuration
        pastTileMaps.Clear();
        futureTileMaps.Clear();

        float currentXPos = 0;

        // Recreate all past and future rooms
        for (int i = 0; i < levelConfig.pastRooms.Length; i++)
        {
            GameObject newPastRoom = Instantiate(levelConfig.pastRooms[i].TilemapPrefab, new Vector3(currentXPos, 0, 0), Quaternion.identity);
            pastTileMaps.Add(newPastRoom);

            GameObject newFutureRoom = Instantiate(levelConfig.futureRooms[i].TilemapPrefab, new Vector3(currentXPos, 0, 0), Quaternion.identity);
            futureTileMaps.Add(newFutureRoom);

            currentXPos += levelConfig.sizeOfRoomX;
        }

        // Set the initial maps to active based on the current scene
        if (currentScene == SceneType.pastScene)
        {
            EnableTileMapElements(pastMap);
        }
        else if (currentScene == SceneType.futureScene)
        {
            EnableTileMapElements(futureMap);
        }
    }

}
