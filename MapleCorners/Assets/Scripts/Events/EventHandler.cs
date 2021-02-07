// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect,
        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
        bool idleRight, bool idleLeft, bool idleUp, bool idleDown);

public static class EventHandler
{
  public static event MovementDelegate MovementEvent;

  public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect,
        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
        bool idleRight, bool idleLeft, bool idleUp, bool idleDown)
    {
        // execute if there are subscribers
        if (MovementEvent != null)
            MovementEvent(inputX, inputY,
                isWalking, isRunning, isIdle, isCarrying,
                toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                idleRight, idleLeft, idleUp, idleDown);
    }

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

  //Time related events

  //Increment the game minute
  public static event Action<int, Season, int, Weekday, int, int, int> AdvanceGameMinuteEvent;

  public static void CallAdvanceGameMinuteEvent(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    if (AdvanceGameMinuteEvent != null)
    {
      AdvanceGameMinuteEvent(year, season, day, weekday, hour, minute, second);
    }
  }

  //Increment the game hour
  public static event Action<int, Season, int, Weekday, int, int, int> AdvanceGameHourEvent;

  public static void CallAdvanceGameHourEvent(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    if (AdvanceGameHourEvent != null)
    {
      AdvanceGameHourEvent(year, season, day, weekday, hour, minute, second);
    }
  }

  //Increment the game day
  public static event Action<int, Season, int, Weekday, int, int, int> AdvanceGameDayEvent;

  public static void CallAdvanceGameDayEvent(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    if (AdvanceGameDayEvent != null)
    {
      AdvanceGameDayEvent(year, season, day, weekday, hour, minute, second);
    }
  }

  /******************************Stubbed for later release***********************************/
  //Increment the game season
  public static event Action<int, Season, int, Weekday, int, int, int> AdvanceGameSeasonEvent;

  public static void CallAdvanceGameSeasonEvent(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    if (AdvanceGameSeasonEvent != null)
    {
      AdvanceGameSeasonEvent(year, season, day, weekday, hour, minute, second);
    }
  }

  //Increment the game year
  public static event Action<int, Season, int, Weekday, int, int, int> AdvanceGameYearEvent;

  public static void CallAdvanceGameYearEvent(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    if (AdvanceGameYearEvent != null)
    {
      AdvanceGameYearEvent(year, season, day, weekday, hour, minute, second);
    }
    }
    // -------------------------------- Scene Events --------------------------------
    // Before Scene Unload Fade Out Event
    public static event Action BeforeSceneUnloadFadeOutEvent;   // Subscriber event
    public static void CallBeforeSceneUnloadFadeOutEvent()      // Method for publisher to call
    {
        if (BeforeSceneUnloadFadeOutEvent != null)
        {
            BeforeSceneUnloadFadeOutEvent();
        }
    }

    // Before Scene Unload Event
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        if (BeforeSceneUnloadEvent != null)
        {
            BeforeSceneUnloadEvent();
        }
    }

    // After Scene Loaded Event
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        if (AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
    }

    // After Scene Load Fade In Event
    public static event Action AfterSceneLoadFadeInEvent;
    public static void CallAfterSceneLoadFadeInEvent()
    {
        if (AfterSceneLoadFadeInEvent != null)
        {
            AfterSceneLoadFadeInEvent();
        }
    }
    // -------------------------------- END Scene Events --------------------------------
}
