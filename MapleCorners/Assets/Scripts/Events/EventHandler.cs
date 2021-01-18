// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{ 

  public static event Action BeforeUnloadSceneEvent;
  public static void CallBeforeUnloadSceneEvent()
  {
    if (BeforeUnloadSceneEvent != null)
    {
      BeforeUnloadSceneEvent();
    }
  }

  public static event Action AfterLoadSceneEvent;
  public static void CallAfterLoadSceneEvent()
  {
    if (AfterLoadSceneEvent != null)
    {
      AfterLoadSceneEvent();
    }
  }

  public static event Action BeforeUnloadSceneFadeEvent;
  public static void CallBeforeUnloadSceneFadeEvent()
  {
    if (BeforeUnloadSceneFadeEvent != null)
    {
      BeforeUnloadSceneFadeEvent();
    }
  }

  public static event Action AfterLoadSceneFadeEvent;

  public static void CallAfterLoadSceneFadeEvent()
  {
    if (AfterLoadSceneFadeEvent != null)
    {
      AfterLoadSceneFadeEvent();
    }
  }
}
