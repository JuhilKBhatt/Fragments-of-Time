using System;
using System.Collections;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class CutsceneScript : MonoBehaviour
{
    public TMP_Text textInput;
    public bool activateText = false;
    public bool cutsceneActive = false;
    //public bool isBOLD = false;
    public CutsceneData cutscene;
    public int currentTextBox = -1;
    public bool isActionHappening;
    public Coroutine currentRoutine;
    public bool skipText = false;
    public GameObject[] cutsceneMoveToPositions;
    public bool enableIconAfterEnd = false;

    public string[] cutsceneAnimations;
    public Animator playerAnimator;
    public bool startCutscene = false;
    private Image panel;
    private bool Aniamting;
    private Transform player;
    public bool isStartOfLevel = false;
    private GameObject[] ContinueObjects;
    private System.Collections.Generic.List<GameObject> objectsInstanciated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (isStartOfLevel)
        {
            if (FindAnyObjectByType<LevelLoader>().currentLevel != 0)
            {
                Destroy(this.gameObject);
            }
        }
        objectsInstanciated = new System.Collections.Generic.List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        panel = GameObject.FindGameObjectWithTag("SignPannel").GetComponent<Image>();
        ContinueObjects = GameObject.FindGameObjectsWithTag("ExtraSign");
        textInput = GameObject.FindGameObjectWithTag("SignInfo").GetComponent<TMP_Text>();
        Invoke("disableContinue", 0.1f);

    }
    void disableContinue()
    {
        foreach (GameObject obj in ContinueObjects)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneActive)
        {
            if (activateText)
            {
                if (currentRoutine == null)
                {
                    currentTextBox += 1;
                    if (cutscene.isCutsceneAction[currentTextBox])
                    {
                        panel.enabled = false;
                        textInput.enabled = false;
                        activateText = false;
                        foreach (GameObject obj in ContinueObjects)
                        {
                            obj.SetActive(false);
                        }
                        Aniamting = true;
                        if (cutscene.actions[currentTextBox] == cutsceneActionType.PlayerMovement)
                        {
                            currentRoutine = StartCoroutine(moveBetweenPositions(currentTextBox));
                        }
                        else if (cutscene.actions[currentTextBox] == cutsceneActionType.ObjectInstantation)
                        {
                            objectsInstanciated.Add(Instantiate(cutscene.objectsToInstantiateInScene[currentTextBox], cutsceneMoveToPositions[currentTextBox].transform));
                            currentRoutine = StartCoroutine(fadeInCreatedObjects());
                        }
                        else if (cutscene.actions[currentTextBox] == cutsceneActionType.ObjectRemoval)
                        {
                            currentRoutine = StartCoroutine(fadeOutCreatedObjects((int) cutscene.thignsToDo[currentTextBox].DoAction()));
                        }
                        else if (cutscene.actions[currentTextBox] == cutsceneActionType.Event)
                        {
                            currentRoutine = StartCoroutine(waitForSecond(cutscene.thignsToDo[currentTextBox].DoAction()));
                        }
                        //currentRoutine = StartCoroutine(moveBetweenPositions(currentTextBox));
                        Debug.Log("A CUTSCENE WOULD PLAY HERE< THIS WOULD NOT BE SKIPPABLE");
                    }
                    else
                    {
                        panel.enabled = true;
                        textInput.enabled = true;
                        foreach (GameObject obj in ContinueObjects)
                        {
                            obj.SetActive(true);
                        }
                        skipText = false;
                        currentRoutine = StartCoroutine(updateTextBox(cutscene.cutsceneText[currentTextBox]));
                        activateText = false;
                    }

                }
                else
                {
                    if (skipText == false && !cutscene.isCutsceneAction[currentTextBox])
                    {
                        skipText = true;
                        StopCoroutine(currentRoutine);
                        currentRoutine = null;
                        textInput.text = cutscene.cutsceneText[currentTextBox];
                        activateText = false;

                    }
                    else
                    {
                        skipText = false;
                    }
                }
            }
            else if (Aniamting == false)
            {
                if (Keyboard.current.fKey.wasPressedThisFrame)
                {
                    if (currentTextBox == cutscene.cutsceneText.Length - 1)
                    {
                        endCutscene();
                    }
                    else
                    {
                        activateText = true;
                    }
                }
            }
        }

    }

    public IEnumerator updateTextBox(string text)
    {
        textInput.text = "";
        bool isBold = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '*')
            {


                if (isBold)
                    textInput.text += "</b>";
                else
                    textInput.text += "<b>";

                isBold = !isBold;
            }
            else
            {
                textInput.text += text[i];
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
        Debug.Log(textInput.text);
        currentRoutine = null;
        Aniamting = false;
        yield return null;
    }
    public IEnumerator waitForSecond(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        currentRoutine = null;
        activateText = true;
        Aniamting = false;
        if (currentTextBox == cutscene.cutsceneText.Length - 1)
        {
            endCutscene();
        }
    }
    public IEnumerator fadeInCreatedObjects()
    {
        float progress = 0;
        if (objectsInstanciated[objectsInstanciated.Count - 1].tag == "BoostInstructions")
            progress = 1;
        while (progress < 1)
            {
                Color currentColor = objectsInstanciated[objectsInstanciated.Count - 1].GetComponent<SpriteRenderer>().color;
                objectsInstanciated[objectsInstanciated.Count - 1].GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, progress);
                progress += Time.deltaTime;
                yield return null;
            }
        yield return null;
        currentRoutine = null;
        activateText = true;
        Aniamting = false;
        yield return null;
    }
    public IEnumerator fadeOutCreatedObjects(int whichObject)
    {
        float progress = 1;
        while (progress > 0)
        {
            Color currentColor = objectsInstanciated[whichObject].GetComponent<SpriteRenderer>().color;
            objectsInstanciated[whichObject].GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, progress);
            progress -= Time.deltaTime;
            yield return null;
        }
        currentRoutine = null;
        activateText = true;
        Aniamting = false;
        Destroy(objectsInstanciated[whichObject]);
        yield return null;
    }
    public IEnumerator moveBetweenPositions(int i)
    {

        Vector3 startPosition = player.position;
        float startTime = Time.time;
        float speed = 2.5f;
        Debug.Log("WE ARE TRYING TO MOVE THE PLAYER");
        if (cutsceneMoveToPositions[i].transform.position.x < startPosition.x)
        {
            speed = -2.5f;
        }

        float JourneyLength = Vector2.Distance(startPosition, cutsceneMoveToPositions[i].transform.position);
        while (Vector2.Distance(player.position, cutsceneMoveToPositions[i].transform.position) >= 0.5f)
        {
            player.GetComponent<PlayerMovement>().playerMoveWalkForAnimations(new Vector2(speed, 0));
            yield return null;
        }
        currentRoutine = null;
        player.GetComponent<PlayerMovement>().stopMoveAniamtions();
        activateText = true;
        Aniamting = false;
        yield return null;
    }
    public IEnumerator moveEvilPlayer(int i, Transform evilPlayer)
    {

        Vector3 startPosition = evilPlayer.position;
        float startTime = Time.time;
        float speed = 1;
        Debug.Log("WE ARE TRYING TO MOVE THE PLAYER");

        float JourneyLength = Vector2.Distance(startPosition, cutsceneMoveToPositions[i].transform.position);
       // while (Vector2.Distance(evilPlayer.position, cutsceneMoveToPositions[i].transform.position) >= 0.5f)
       // {
        //    evilPlayer.GetComponent<PlayerMovement>().playerMoveWalkForAnimations(new Vector2(2.5f, 0));
       //     yield return null;
       // }
        currentRoutine = null;
        evilPlayer.GetComponent<ShadowPlayerMovement>().stopMoveAniamtions();
        activateText = true;
        Aniamting = false;
        yield return null;
    }

    public void beginCutscene()
    {
        activateText = true;
        Application.runInBackground = true;
        player.GetComponent<EchoAbility>().inputDisabled = true;
        GameObject.FindGameObjectWithTag("SceneManagerObject").GetComponent<PrefabSceneManager>().inputDisabled = true;

    }
    private void endCutscene()
    {
        activateText = false;
        Application.runInBackground = false;
        player.GetComponent<PlayerMovement>().disableInput = false;
        player.GetComponent<EchoAbility>().inputDisabled = false;
        GameObject.FindGameObjectWithTag("SceneManagerObject").GetComponent<PrefabSceneManager>().reEneableInput(enableIconAfterEnd);
        panel.enabled = false;
        textInput.enabled = false;
        foreach (GameObject obj in ContinueObjects)
        {
            obj.SetActive(false);
        }
        if (objectsInstanciated.Count != 0)
        {
            foreach (GameObject objec in objectsInstanciated)
            {
                if(objec.tag != "BoostInstructions")
                Destroy(objec);
            }
        }
        Destroy(this.gameObject);
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            cutsceneActive = true;
            beginCutscene();
            player.GetComponent<PlayerMovement>().CutscenHasBegun();
                

            }
    }
}
