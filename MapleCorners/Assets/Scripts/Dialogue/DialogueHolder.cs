using UnityEngine;
using System.Collections;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(dialogueSequence());
        }
        // Iterates through each line of dialogue to only show the current one (active vs. inactive)
        private IEnumerator dialogueSequence()
        {
            for( int i=0; i<transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            // Deactivate holder when completed
            gameObject.SetActive(false);
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
