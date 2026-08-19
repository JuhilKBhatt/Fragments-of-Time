
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class Level4Resetting : MonoBehaviour
{
    public float resetInterval = 10f;
    private float resetTimer;

    private PrefabSceneManager sceneManager;
    public bool shouldReset = false;
    private GameObject spawnPoint; // Cache SpawnPoint
    public ShaderInAndOut glitchScreen;

    [SerializeField] private LevelObject level4Object; // Drag Level4 LevelObject here in the inspector

    // UI elements for countdown and warning
    [SerializeField] private TextMeshProUGUI countdownText; // Reference to the countdown text UI element
    [SerializeField] private GameObject warningPanel; // UI panel to display warning
    [SerializeField] private float warningThreshold = 3f; // Time before reset when the warning should show up

    private Dictionary<GameObject, Vector3> initialPushablePositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Vector3> initialRollablePositions = new Dictionary<GameObject, Vector3>();
    private List<GameObject> leversList = new List<GameObject>();
    public bool dontResetWeInCutscene = false;
    public AudioSource resetSound;

    void Start()
    {
        resetTimer = resetInterval;

        sceneManager = FindFirstObjectByType<PrefabSceneManager>();

        if (sceneManager == null)
        {
            Debug.LogWarning("PrefabSceneManager not found. Level reset will not work.");
            return;
        }

        if (sceneManager.levelConfig == level4Object)
        {
            //if(sceneManager.levelConfig.number)
            //shouldReset = true;
            Debug.Log("Level4 detected. Starting reset timer.");

            // Cache the SpawnPoint
            spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint == null)
            {
                Debug.LogWarning("SpawnPoint not found in scene.");
            }
            else
            {
                // Move player to SpawnPoint immediately
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // player.transform.position = spawnPoint.transform.position;
                    Debug.Log("Player moved to SpawnPoint on scene load.");
                }
                else
                {
                    Debug.LogWarning("Player not found in scene on load.");
                }
            }

            // Store initial positions of PushableBox objects
            GameObject[] pushables = GameObject.FindGameObjectsWithTag("PushableBox");
            foreach (GameObject pushable in pushables)
            {
                initialPushablePositions.Add(pushable, pushable.transform.position);
                Rigidbody2D rb = pushable.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }

            // Store initial positions of RollableObject objects
            GameObject[] rollables = GameObject.FindGameObjectsWithTag("RollableObject");
            foreach (GameObject rollable in rollables)
            {
                initialRollablePositions.Add(rollable, rollable.transform.position);
                Rigidbody2D rb = rollable.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }
            GameObject[] levers = GameObject.FindGameObjectsWithTag("Lever");
            foreach (GameObject lever in levers)
            {
                leversList.Add(lever);
            }
        }
        else
        {
            Debug.Log("Not Level4. No reset timer started.");
        }

        // Ensure the UI is hidden at the start
        if (warningPanel != null)
        {
            warningPanel.SetActive(false); // Initially hide the warning panel
        }
    }

    void Update()
    {
        if (!shouldReset) return;

        resetTimer -= Time.deltaTime;

        // Update the UI countdown using TextMeshPro
        if (countdownText != null)
        {
            countdownText.text = Mathf.Ceil(resetTimer).ToString() + "s";
        }

        // Show the warning panel when the timer reaches the threshold
        if (resetTimer <= warningThreshold && warningPanel != null)
        {
            warningPanel.SetActive(true); // Show warning panel
        }

        if (resetTimer <= 0f)
        {
            ResetRoom();
            resetTimer = resetInterval;
            warningPanel.SetActive(false); // Hide the warning panel after the reset
        }
    }

    private void ResetRoom()
    {
        Debug.Log("Resetting movable objects with fade...");
        PlayAudio();
        StartCoroutine(ResetWithFade());
        if (glitchScreen != null)
        {
            glitchScreen.ActiateShader();
        }
    }
    public void resetForCutscene()
    {
        foreach (GameObject lever in leversList)
        {
            if (lever.GetComponent<ButtonTriggerMultiple>())
                lever.GetComponent<ButtonTriggerMultiple>().initalDoor();
        }
    }

    private System.Collections.IEnumerator ResetWithFade()
    {
        float fadeOutDuration = 0.3f; // Adjust as needed
        float fadeInDelay = 0.1f;     // Delay before fading in
        float fadeInDuration = 0.3f;  // Adjust as needed

        // Fade out PushableBox objects
        foreach (GameObject lever in leversList)
        {
            if (lever.GetComponent<ButtonTriggerMultiple>())
            {
                lever.GetComponent<ButtonTriggerMultiple>().initalDoor();
                Debug.Log("BAZINGA AA");
            }
            else
            {
                Debug.Log("Bazonga");
            }
        }
        List<Coroutine> fadeOutCoroutines = new List<Coroutine>();
        foreach (KeyValuePair<GameObject, Vector3> pair in initialPushablePositions)
        {
            if (pair.Key != null)
            {
                GameObjectFader fader = pair.Key.GetComponent<GameObjectFader>();
                if (fader != null && pair.Key.GetComponent<SpriteRenderer>().color.a != 0)
                {
                    fadeOutCoroutines.Add(StartCoroutine(FadeOut(fader, fadeOutDuration)));
                }
            }
        }

        // Fade out RollableObject objects
        foreach (KeyValuePair<GameObject, Vector3> pair in initialRollablePositions)
        {
            if (pair.Key != null)
            {
                GameObjectFader fader = pair.Key.GetComponent<GameObjectFader>();
                if (fader != null && pair.Key.GetComponent<SpriteRenderer>().color.a != 0)
                {
                    fadeOutCoroutines.Add(StartCoroutine(FadeOut(fader, fadeOutDuration)));
                }
            }
        }

        // Wait for all fade-out animations to complete
        foreach (var coroutine in fadeOutCoroutines)
        {
            yield return coroutine;
        }

        // Reset positions and velocities
        foreach (KeyValuePair<GameObject, Vector3> pair in initialPushablePositions)
        {
            if (pair.Key != null)
            {
                pair.Key.transform.position = pair.Value;
                Rigidbody2D rb = pair.Key.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.Sleep();
                }
            }
        }

        foreach (KeyValuePair<GameObject, Vector3> pair in initialRollablePositions)
        {
            if (pair.Key != null)
            {
                pair.Key.transform.position = pair.Value;
                Rigidbody2D rb = pair.Key.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.Sleep();
                }
            }
        }

        // Wait for a short delay before fading in
        yield return new WaitForSeconds(fadeInDelay);

        // Fade in PushableBox objects
        List<Coroutine> fadeInCoroutines = new List<Coroutine>();
        foreach (KeyValuePair<GameObject, Vector3> pair in initialPushablePositions)
        {
            if (pair.Key != null)
            {
                GameObjectFader fader = pair.Key.GetComponent<GameObjectFader>();
                if (fader != null && pair.Key.GetComponent<BoxCollider2D>().isTrigger == false)
                {
                    fadeInCoroutines.Add(StartCoroutine(FadeIn(fader, fadeInDuration)));
                }
            }
        }

        // Fade in RollableObject objects
        foreach (KeyValuePair<GameObject, Vector3> pair in initialRollablePositions)
        {
            if (pair.Key != null)
            {
                GameObjectFader fader = pair.Key.GetComponent<GameObjectFader>();
                if (fader != null && pair.Key.GetComponent<CircleCollider2D>().isTrigger == false)
                {
                    fadeInCoroutines.Add(StartCoroutine(FadeIn(fader, fadeInDuration)));
                }
            }
        }

        // Wait for all fade-in animations to complete
        foreach (var coroutine in fadeInCoroutines)
        {
            yield return coroutine;
        }

        // Optionally trigger a visual glitch if you still want that effect
        
    }

    private System.Collections.IEnumerator FadeOut(GameObjectFader fader, float duration)
    {
        if (fader != null)
        {
            float time = 0f;
            SpriteRenderer sr = fader.GetComponent<SpriteRenderer>();
            Color startColor = sr.color;
            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / duration);
                sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }
    }

    private System.Collections.IEnumerator FadeIn(GameObjectFader fader, float duration)
    {
        if (fader != null)
        {
            float time = 0f;
            SpriteRenderer sr = fader.GetComponent<SpriteRenderer>();
            Color startColor = sr.color;
            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / duration);
                sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        }
    }
    public void PlayAudio()
    {
        resetSound.Play();
    }
    public void ActivateGlitchCutscene()
    {
        glitchScreen.ActiateShader();
        PlayAudio();
    }
}