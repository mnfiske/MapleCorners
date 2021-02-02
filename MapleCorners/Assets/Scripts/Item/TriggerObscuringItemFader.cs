using UnityEngine;

// attached to player object to trigger
// collision fading
public class TriggerObscuringItemFader : MonoBehaviour
{
    // Fade Out when entering collission
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the object collided with, then get fader components
        // + children and trigger fade out
        ObscuringItemFader[] obscuringItemFader =
            collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();
        if (obscuringItemFader.Length > 0)
        {
            for (int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].FadeOut();
            }
        }
    }

    // fade In after exiting collission
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Get object and children
        ObscuringItemFader[] obscuringItemFader =
            collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();
        if (obscuringItemFader.Length > 0)
        {
            for (int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].FadeIn();
            }
        }
    }
}
