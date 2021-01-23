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
}
