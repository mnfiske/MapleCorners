// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehavior<SceneControllerManager>
{
  // Are we currently fading? Gets set to true on fade start, flase on fade finish
  private bool isFadingActive;
  // The amount of time, in seconds, the fade effect will last
  [SerializeField] private float fadeLength = 1;
  // Link to the FadeImage CanvasGroup 
  [SerializeField] private CanvasGroup fadeImageCanvasGroup = null;
  // Link to the actual image within the FadeImage CanvasGroup
  [SerializeField] private Image fadeImage;
  // The name of the scene the game starts on
  public SceneName openingSceneName;

  /// <summary>
  /// Fade out of the current scene and load the destination scene
  /// </summary>
  /// <param name="destinationName">Name of the scene to load</param>
  /// <param name="destinationPosition">The position the player should spawn in, in the destination scene</param>
  //private IEnumerator FadeAndTransition(string destinationName, Vector3 destinationPosition)
  //{
  //  // Publish that the unload scene and fade event is about to be triggered
  //  EventHandler.CallBeforeUnloadSceneFadeEvent();

  //  // Fade the screen
  //  yield return StartCoroutine(Fade(1));

  //  Player
  //}

  /// <summary>
  /// Fade out of the current scene and load the destination scene
  /// </summary>
  /// <param name="destinationName">Name of the scene to load</param>
  /// <param name="destinationPosition">The position the player should spawn in, in the destination scene</param>
  //public void transitionScene(string destinationName, Vector3 destinationPosition)
  //{
  //  // Only call FadeAndTransition if we're not already executing a fade
  //  if (!isFadingActive)
  //  {
  //    //StartCoroutine(FadeAndTransition(destinationName, destinationPosition));
  //  }
  //}

  /************************************************************************************
   ADAM: These methods should be good to go--commented them out because they rely on some methods that aren't created yet.
  private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
  {
    EventHandler.CallBeforeUnloadSceneFadeEvent();

    yield return StartCoroutine(Fade(1f));

    // Store the current scene data
    SaveLoadManager.Instance.StoreCurrentSceneData();

    Player.Instance.gameObject.transform.position = spawnPosition;

    EventHandler.CallBeforeUnloadSceneEvent();

    yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

    EventHandler.CallAfterLoadSceneEvent();

    // Restore the new scene data
    SaveLoadManager.Instance.RestoreCurrentSceneData();

    yield return StartCoroutine(Fade(0f));

    EventHandler.CallAfterLoadSceneFadeEvent();
  }

  private IEnumerator Start()
  {
    fadeImage.color = new Color(0f, 0f, 0f, 1f);
    fadeImageCanvasGroup.alpha = 1f;

    yield return StartCoroutine(LoadSceneAndSetActive(openingSceneName.ToString()));

    EventHandler.CallAfterLoadSceneEvent();

    // Restore the current scene data
    SaveLoadManager.Instance.RestoreCurrentSceneData();

    StartCoroutine(Fade(0f));

  }
  */
}
