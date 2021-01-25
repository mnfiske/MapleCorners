using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        // 0 until all text has been delayed
        public bool finished { get; private set; }

        // Writes text one character at a time with a customizable color, font, and delay speed
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay, AudioClip sound)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;

            for(int i=0; i<input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }

            // Wait for a little bit until next line is shown
            yield return new WaitUntil(()=> Input.GetMouseButton(0));
            finished = true;
        }
    }
}
