// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehavior<SceneControllerManager>
{
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    // From Enum scene names
    public SceneName startingSceneName;

    // Controls fading in and out
    private IEnumerator Fade(float finalAlpha)  // finalAlpha set in Unity but probably 1
    {
        // set the fading flag to avoid running routine again
        isFading = true;

        // block input via RayCast
        faderCanvasGroup.blocksRaycasts = true;

        // Speed of fade
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // perfrom fade
        while(!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            // Wait for next frame
            yield return null;
        }

        // Set flag and remove blocking
        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }

    // Coroutine for fading
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        // Call before scene unload fade out event
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();

        // Start fading to black
        yield return StartCoroutine(Fade(1f));

        // Set Player Position
        Player.Instance.gameObject.transform.position = spawnPosition;

        // Call before scene unload event
        EventHandler.CallBeforeSceneUnloadEvent();

        // Unload current scene - asynchronis to avoid game freezing
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Start loading the given scene
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // call after scene load event
        EventHandler.CallAfterSceneLoadEvent();

        // Start fade in
        yield return StartCoroutine(Fade(0f));

        // Call after scene load fade in event
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Load game and add to already loaded scene (persistent scene)
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Find the  scene that was recently loaded
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as active
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Start()
    {
        // set the initial alpha to start fully black
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        // Begin loading first scene
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        // Call events for subscribers
        EventHandler.CallAfterSceneLoadEvent();

        // Start Fade in
        StartCoroutine(Fade(0f));
    }

    // This is the main external point of contact and influence from the rest of the project
    // this will be called when the player wants to switch scenes.
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        // If a fade isn't happening then start fading and switching scenes
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }
    
}
