// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
}
