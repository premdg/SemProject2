using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FadeText : MonoBehaviour
{
    public CanvasGroup canvasGroup;  
    public TextMeshProUGUI textMeshPro;  // Reference to the TextMeshPro component
    public float fadeDuration = 5f;   // Duration of the fade-in and fade-out animations
    public float displayDuration = 5f; // Time the text stays fully visible (set to 5 seconds)

    void Start()
    {
        // Start the fade-in, display, and fade-out process
        StartCoroutine(FadeTextInAndOut());
    }

    IEnumerator FadeTextInAndOut()
    {
        // Fade in
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        // Wait for the display duration (5 seconds)
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return StartCoroutine(Fade(1, 0, fadeDuration));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;  // Wait for the next frame
        }

        canvasGroup.alpha = endAlpha;  // Ensure it fully fades in or out
    }
}
