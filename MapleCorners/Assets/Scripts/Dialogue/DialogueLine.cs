using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;

        [Header ("Text Options")]
        // Serialize allows Unity to edit
        [SerializeField] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private Font textFont;

        [Header("Time Parameters")]
        [SerializeField] private float delay;

        [Header("Sound")]
        [SerializeField] private AudioClip sound;

        // Awake function
        private void Awake()
        {
            textHolder = GetComponent<Text>();
            // Empty if something has been added in Unity
            textHolder.text = "";
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay, sound));
        }
    }
}