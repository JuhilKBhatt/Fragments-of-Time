using UnityEngine;
using System.Collections;

public class GameObjectFader : MonoBehaviour
{
    public float fadeDuration = 1.0f; // fade duration

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject!");
            enabled = false;
            return;
        }
        originalColor = spriteRenderer.color;
    }

    public void FadeOutAndIn(System.Action onComplete = null)
    {
        StartCoroutine(FadeSequence(onComplete));
    }

    private IEnumerator FadeSequence(System.Action onComplete)
    {
        // Fade Out
        if(spriteRenderer.color.a != 0)
        {
        yield return StartCoroutine(Fade(1f, 0f));

        // Action in between fades (if any)
        onComplete?.Invoke();

        // Fade In
        //if(spriteRenderer.gameObject.GetComponent<)
        yield return StartCoroutine(Fade(0f, 1f));
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color currentColor = spriteRenderer.color;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            Color newColor = currentColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
            yield return null;
        }
        // Ensure the final alpha is set
        Color finalColor = currentColor;
        finalColor.a = endAlpha;
        spriteRenderer.color = finalColor;
    }
}