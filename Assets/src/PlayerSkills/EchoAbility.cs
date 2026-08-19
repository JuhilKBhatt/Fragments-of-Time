using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System;

public class EchoAbility : MonoBehaviour
{
    [SerializeField] private EchoData echoData;
    [SerializeField] private GameObject echoVisualPrefab;
    [SerializeField] private float holdThreshold = 1f;
    [SerializeField] private ParticleSystem echoboostEffect;
    public Material playerGlitch;

    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject currentEchoVisual;

    private float eKeyHoldTime = 0f;
    private bool isHolding = false;
    public bool canEcho = false;
    public bool canCreateEcho = true;
    public bool inputDisabled = false;
    private float timeToAllowBoost;
    private bool allowForEchoBoostEffect = false;
    private bool allowEchoBoostingEffect = false;
    private float timeToCheckForJump = 0;
    private bool lastEchoBoostInAir = false;
    public Image echoIndicator;
    public Image echoIndicatorPrompt;

    private void Start()
    {
      //  echoIndicator = GameObject.FindGameObjectWithTag("EchoIndicator").GetComponent<Image>();
       // echoIndicatorPrompt = GameObject.FindGameObjectWithTag("EchoIndicatorPrompt").GetComponent<Image>();
        animator = GetComponent<Animator>();
        playerGlitch.SetInt("_ActivateGlitch", 0);
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        
        ResetEchoData();
    }

    private void Update()
    {
        if (!inputDisabled)
        {
            if (canEcho == true && Time.timeScale != 0)
            {
                if (Keyboard.current.eKey.isPressed)
                {
                    eKeyHoldTime += Time.deltaTime;

                    if (eKeyHoldTime > holdThreshold && !isHolding && echoData.hasEcho)
                    {
                        isHolding = true;
                        RemoveEcho(); // Just remove the echo, no teleport
                        canCreateEcho = true;
                    }
                }

                if (Keyboard.current.eKey.wasReleasedThisFrame)
                {
                    if (!isHolding)
                    {
                        if (!echoData.hasEcho)
                            SaveEcho();
                        else if (echoData.hasEcho)
                        {
                            if (canCreateEcho == true)
                            {
                                RecallEcho();
                            }
                            else
                            {
                                this.GetComponent<PlaySoundEffect>().PlaySFX(4);
                            }
                        }

                    }

                    eKeyHoldTime = 0f;
                    isHolding = false;
                }

            }
            if ((Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) && allowForEchoBoostEffect && !echoboostEffect.isPlaying && lastEchoBoostInAir == false)
            {
                if (timeToAllowBoost >= Time.time)
                {
                    echoboostEffect.Play();
                    lastEchoBoostInAir = true;
                    allowForEchoBoostEffect = false;
                    allowEchoBoostingEffect = false;
                }
                else
                {
                    allowForEchoBoostEffect = false;
                    allowEchoBoostingEffect = false;
                }
            }
            if ((Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) && allowEchoBoostingEffect && lastEchoBoostInAir == false)
            {
                timeToCheckForJump = Time.time + 0.3f;
            }

        }
    }

    public void isGroundedAgain()
    {
        lastEchoBoostInAir = false;
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
        if (this.GetComponent<PlayerMovement>().isPlayerGrounded())
        {
            allowEchoBoostingEffect = false;
        }
        else
        {
            allowEchoBoostingEffect = true;
        }
        CreateEchoVisual();
    }

    private void RecallEcho()
    {
        playerGlitch.SetInt("_ActivateGlitch", 1);
        Invoke("removeGlitch", 0.3f);
        this.GetComponent<PlaySoundEffect>().RandomisePitch(3, 1f, 0.9f);
        this.GetComponent<PlaySoundEffect>().PlaySFX(3);
        transform.position = echoData.savedPosition;
        animator.Play(echoData.savedAnimationHash, 0);
        echoData.hasEcho = false;
        blackOutIndicator(false);
        setIndicatorsActive();
        if (allowEchoBoostingEffect)
        {
            allowForEchoBoostEffect = true;
        }
        if (timeToCheckForJump >= Time.time && allowEchoBoostingEffect && lastEchoBoostInAir == false)
        {
            allowEchoBoostingEffect = false;
            allowForEchoBoostEffect = false;
            echoboostEffect.Play();
            lastEchoBoostInAir = true;

        }
        timeToAllowBoost = Time.time + 0.3f;

        if (currentEchoVisual != null)
            StartCoroutine(FadeOutAndDestroy(currentEchoVisual));
    }

    public void RemoveEcho()
    {
        echoData.hasEcho = false;

        if (currentEchoVisual != null)
        {
            StartCoroutine(FadeOutAndDestroy(currentEchoVisual));
            blackOutIndicator(false);
            setIndicatorsActive();
        }
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
                echoSpriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Start transparent
                StartCoroutine(FadeIn(echoSpriteRenderer));
            }
            blackOutIndicator(true);
        }
    }
    private void removeGlitch()
    {
        playerGlitch.SetInt("_ActivateGlitch", 0);
    }

    private IEnumerator FadeIn(SpriteRenderer spriteRenderer)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.5f, elapsed / duration);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Final value
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
        if (e)
            setIndicatorsActive();
        else
            setIndicatorsInactive();
    }

    public bool HasPlacedEcho()
    {
        return echoData != null && echoData.hasEcho;
    }

    public void setIndicatorsActive()
    {
        echoIndicatorPrompt.color = new Color(1, 1, 1, 0.5f);
    }
    public void setIndicatorsInactive()
    {
        echoIndicatorPrompt.color = new Color(1, 1, 1, 0);
    }
    public void blackOutIndicator(bool yorn)
    {
        if (yorn)
            echoIndicator.color = new Color(0, 0, 0);
        else
            echoIndicator.color = new Color(1, 1, 1);

    }

    public void DisableStuffAtBegin()
    {
        echoIndicator.color = new Color(1, 1, 1, 0);
        foreach (Image img in echoIndicator.GetComponentsInChildren<Image>())
        {
            img.color = new Color(1, 1, 1, 0);
        }
        
    }

    public void enableStuffAgain()
    {
        echoIndicator.color = new Color(1, 1, 1, 1);
        foreach (Image img in echoIndicator.GetComponentsInChildren<Image>())
        {
            img.color = new Color(1, 1, 1, 1);
        }
        echoIndicatorPrompt.color = new Color(1, 1, 1, 0.5f);

    }
}