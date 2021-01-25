using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Gives permission to other scripts to get but not set
    public static SoundManager instance { get; private set; }

    private AudioSource source;

    private void Awake()
    {
        instance = this;

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
