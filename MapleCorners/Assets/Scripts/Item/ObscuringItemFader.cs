using System.Collections;
using UnityEngine;

// Requires game object to have Component: Sprite Renderer
[RequireComponent(typeof(SpriteRenderer))]

public class ObscuringItemFader : MonoBehaviour
{
    // cache sprite renderer
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Gets current sprite renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Trigger fade out behavior and co-routine (runs for frame)
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    // co-routine - FadeOut
    private IEnumerator FadeInRoutine()
    {
        // get current alpha
        float currentAlpha = spriteRenderer.color.a;

        // determine target alpha
        float distance = 1f - currentAlpha;

        // fade in every frame until close to target
        while (1f - currentAlpha > 0.01f)
        {
            currentAlpha = currentAlpha + distance /
                Settings.fadeInSeconds * Time.deltaTime;
            // Keep colors the same but decrease alpha
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);

            yield return null;
        }

        // Set back to full alpha when while loop is complete
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    // co-routine - FadeOut
    private IEnumerator FadeOutRoutine()
    {
        // get current alpha
        float currentAlpha = spriteRenderer.color.a;

        // determine target alpha
        float distance = currentAlpha - Settings.targetAlpha;

        // fade out every frame until close to target
        while (currentAlpha - Settings.targetAlpha > 0.01f)
        {
            currentAlpha = currentAlpha - distance /
                Settings.fadeOutSeconds * Time.deltaTime;
            // Keep colors the same but decrease alpha
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);

            yield return null;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, Settings.targetAlpha);
    }
}
