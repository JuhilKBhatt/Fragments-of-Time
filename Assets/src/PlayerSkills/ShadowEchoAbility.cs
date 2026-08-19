using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class ShadowEchoAbility : MonoBehaviour
{
    [SerializeField] private EchoData echoData;
    [SerializeField] private GameObject echoVisualPrefab;
    [SerializeField] private float holdThreshold = 1f;
    public Material playerGlitch;

    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject currentEchoVisual;

    private float eKeyHoldTime = 0f;
    private bool isHolding = false;
    public bool canEcho = false;
    public bool canCreateEcho = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //playerGlitch.SetInt("_ActivateGlitch", 0);
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        ResetEchoData();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
       // playerGlitch.SetInt("_ActivateGlitch", 0);
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        ResetEchoData();
    }

    private void Update()
    {
    }

    public void makeEcho()
    {
        if (!echoData.hasEcho)
            SaveEcho();

    }
    public void returnToEcho()
    {
        if(echoData.hasEcho)
        RecallEcho();
    }

    private void SaveEcho()
    {
        echoData.savedPosition = transform.position;
        echoData.savedAnimationHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        echoData.hasEcho = true;
        this.GetComponent<PlaySoundEffect>().RandomisePitch(2, 0.9f, 1f);
        this.GetComponent<PlaySoundEffect>().setClipToStart(2);
        this.GetComponent<PlaySoundEffect>().PlaySFX(2);
        Debug.LogWarning("WE are creating an echo here");
        CreateEchoVisual();
    }

    private void RecallEcho()
    {
        //playerGlitch.SetInt("_ActivateGlitch", 1);
        //Invoke("removeGlitch", 0.3f);
        this.GetComponent<PlaySoundEffect>().RandomisePitch(2, -1f, -0.9f);
        this.GetComponent<PlaySoundEffect>().setClipToEnd(2);
        this.GetComponent<PlaySoundEffect>().PlaySFX(2);
        transform.position = echoData.savedPosition;
        animator.Play(echoData.savedAnimationHash, 0);
        echoData.hasEcho = false;

        if (currentEchoVisual != null)
            StartCoroutine(FadeOutAndDestroy(currentEchoVisual));
    }

    public void RemoveEcho()
    {
        echoData.hasEcho = false;

        if (currentEchoVisual != null)
            StartCoroutine(FadeOutAndDestroy(currentEchoVisual));
    }

    private void CreateEchoVisual()
    {
        if (echoVisualPrefab != null)
        {
            currentEchoVisual = Instantiate(echoVisualPrefab, transform.position, transform.rotation);
            SpriteRenderer echoSpriteRenderer = currentEchoVisual.GetComponent<SpriteRenderer>();

            if (echoSpriteRenderer != null && playerSpriteRenderer != null)
            {
                echoSpriteRenderer.sprite = playerSpriteRenderer.sprite;
                echoSpriteRenderer.flipX = playerSpriteRenderer.flipX;
                echoSpriteRenderer.color = new Color(0f, 0f, 0f, 0f); // Start transparent
                StartCoroutine(FadeIn(echoSpriteRenderer));
            }
        }
    }
    private void removeGlitch()
    {
        //playerGlitch.SetInt("_ActivateGlitch", 0);
    }

    private IEnumerator FadeIn(SpriteRenderer spriteRenderer)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.5f, elapsed / duration);
            spriteRenderer.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(0f, 0f, 0f, 0.5f); // Final value
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float duration = 0.5f;
            float elapsed = 0f;
            Color originalColor = sr.color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / duration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        Destroy(obj);
    }

    private void ResetEchoData()
    {
        if (echoData != null)
        {
            echoData.hasEcho = false;
            echoData.savedPosition = Vector3.zero;
            echoData.savedAnimationHash = 0;
        }
    }

    public void ResetToEchoOrStart(Vector3 startPosition)
    {
        if (echoData.hasEcho)
        {
            transform.position = echoData.savedPosition;
            animator.Play(echoData.savedAnimationHash, 0);
            echoData.hasEcho = false;

            if (currentEchoVisual != null)
                StartCoroutine(FadeOutAndDestroy(currentEchoVisual));
        }
        else
        {
            transform.position = startPosition;
            animator.Play("IdleAnimation");
        }
    }
    public void canNowEcho()
    {
        canEcho = true;
    }
    public void ableToMakeEcho()
    {
        //canCreateEcho = true;
    }
    public void notAbleToMakeEcho()
    {
       // canCreateEcho = false;
    }

    public void isEchoOverlap(bool e)
    {
        canCreateEcho = e;
    }

    public bool HasPlacedEcho()
    {
        return echoData != null && echoData.hasEcho;
    }
}