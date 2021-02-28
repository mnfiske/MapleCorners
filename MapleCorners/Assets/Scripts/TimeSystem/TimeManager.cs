// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : SingletonMonoBehavior<TimeManager>, ISaveable
{
    //Track the game's time properties
    private int day = 1;
    private int hour = 6;
    private int minute = 30;
    private int second = 0;
    private Weekday weekday = Weekday.Mon;
    private bool clockPaused = false;
    private float tick = 0f;

    private string _iSaveableID;
    public string ISaveableID { get { return _iSaveableID; } set { _iSaveableID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    //Years and seasons are slated to be added in an upcoming release. Add properies here so the methods
    //can be stubbed out to reduce later refactoring
    private int year = 1;
    private Season season = Season.Spring;

    protected override void Awake()
    {
        base.Awake();

        // Get a guid and store it in the ISaveableID
        ISaveableID = GetComponent<GenerateGuid>().Guid;

        GameObjectSave = new GameObjectSave();
    }

    /// <summary>
    /// Call the AdvanceGameMinuteEvent as this component initializes itself
    /// </summary>
    private void Start()
    {
        EventHandler.CallAdvanceGameMinuteEvent(year, season, day, weekday, hour, minute, second);
    }

    private void OnEnable()
    {
        ISaveableRegister();

        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadFadeOut;
        EventHandler.AfterSceneLoadEvent += AfterSceneLoadFadeIn;
    }

    private void OnDisable()
    {
        ISaveableDeregister();

        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadFadeOut;
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoadFadeIn;
    }

    private void BeforeSceneUnloadFadeOut()
    {
        clockPaused = true;
    }

    private void AfterSceneLoadFadeIn()
    {
        clockPaused = false;
    }

    /// <summary>
    /// Returns the current weekday
    /// </summary>
    private Weekday getWeekday()
    {
        //TODO: When implementing years and seasons, we'll have to caculate the total days instead of using the day prop
        int dayOfTheWeek = day % 7;

        return (Weekday)dayOfTheWeek;
    }

    /// <summary>
    /// Increments the second property. Does a cascading check as to whether any other time properties should be incremented
    /// </summary>
    private void updateTime()
    {
        //Debug.Log("seconds: " + second);
        second++;

        //If the number of seconds have added up to a minute, increment minute and reset second to 0
        if (second >= 60)
        {
            second = 0;
            minute++;

            //If the number of minutes have added up to an hour, increment hour and reset minute to 0
            if (minute >= 60)
            {
            minute = 0;
            hour++;

            //If the number of secondhours have added up to a day, increment day and reset hour to 0
            if (hour >= 24)
            {
                hour = 0;
                day++;

                //TODO: When implementing seasons and years, contine to cascade here

                //Update the day of the week
                weekday = getWeekday();
                EventHandler.CallAdvanceGameDayEvent(year, season, day, weekday, hour, minute, second);
            }

            //Update the hour
            EventHandler.CallAdvanceGameHourEvent(year, season, day, weekday, hour, minute, second);
            }

            //Update the minute
            EventHandler.CallAdvanceGameMinuteEvent(year, season, day, weekday, hour, minute, second);

            //Debug.Log("Day: " + day + " hour: " + hour + " minute: " + minute);
        }
    }

    /// <summary>
    /// Updates the seconds and resets the tick, if enough ticks have passed to add up to a second
    /// </summary>
    private void updateTick()
    {
        //Add the deltaTime to the tick
        tick += Time.deltaTime;

        //If the tick has reached the second threshold, update the seconds and subtract that second from the tick
        if (tick >= Settings.secondsPerGameSecond)
        {
            tick -= Settings.secondsPerGameSecond;
            updateTime();
        }
    }

    /// <summary>
    /// Check if the game is currently paused--if not, udate the tick property
    /// </summary>
    private void Update()
    {
        if (!clockPaused)
        {
            updateTick();
        }
    }

    // test script to increment time to 6am
    public void AdvanceToNextDay()
    {
        //Debug.Log("Made it here");
        //Debug.Log("Hour: " + hour + "Minute: " + minute + "second: " + second);
        //Debug.Log("Is this true? " + (hour != 6 && minute != 0 && second != 0) );
        while ( !(hour == 6 && minute == 0 && second == 0) )
        {
            //Debug.Log("incrementing...");
            updateTime();
        }
    }

    /// <summary>
    /// Register the clock with ISaveable
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Add(this);
    }

    /// <summary>
    /// Deregister the clock with ISaveable
    /// </summary>
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Remove(this);
    }

    public GameObjectSave ISaveableSave()
    {
        // Remove existing save data for this scene if it exists
        GameObjectSave.SceneData.Remove(Settings.MainScene);

        SceneSave sceneSave = new SceneSave();

        // Instantiate the dictionary to store the clock data
        sceneSave.clockData = new Dictionary<string, int>();

        // Add values to the int dictionary
        sceneSave.clockData.Add("gameDay", day);
        sceneSave.clockData.Add("gameHour", hour);
        sceneSave.clockData.Add("gameMinute", minute);
        sceneSave.clockData.Add("gameSecond", second);
        sceneSave.clockData.Add("gameDayOfWeek", (int)weekday);

        // Add the clock data to the GameObjectSave
        GameObjectSave.SceneData.Add(Settings.MainScene, sceneSave);

        return GameObjectSave;
    }

    public void ISaveableLoad(GameSave gameSave)
    {
        // Check the save data for the game object
        if (gameSave.gameObjectData.TryGetValue(ISaveableID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // Get the data from the main scene--this is where the clock is stored
            if (GameObjectSave.SceneData.TryGetValue(Settings.MainScene, out SceneSave sceneSave))
            {
                // Check for the clock data
                if (sceneSave.clockData != null)
                {
                    // Get the clock data 

                    if (sceneSave.clockData.TryGetValue("gameDay", out int savedGameDay))
                        day = savedGameDay;

                    if (sceneSave.clockData.TryGetValue("gameHour", out int savedGameHour))
                        hour = savedGameHour;

                    if (sceneSave.clockData.TryGetValue("gameMinute", out int savedGameMinute))
                        minute = savedGameMinute;

                    if (sceneSave.clockData.TryGetValue("gameSecond", out int savedGameSecond))
                        second = savedGameSecond;

                    if (sceneSave.clockData.TryGetValue("gameDayOfWeek", out int savedGameDayOfWeek))
                        weekday = (Weekday)savedGameDayOfWeek;

                    // Clear the tick
                    tick = 0f;

                    // Advance the game minute even to set the game clock in the UI
                    EventHandler.CallAdvanceGameMinuteEvent(year, season, day, weekday, hour, minute, second);
                }
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // No scene data to store
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // No scene data to restore
    }

    #region Test Methods
    public void TestAdvanceGameMinute()
    {
        int secondsInMinute = 60;

        for (int i = 0; i < secondsInMinute; i++)
        {
            updateTime();
        }
    }

    public void TestAdvanceGameDay()
    {
        int secondsInDay = 86400;

        for (int i = 0; i < secondsInDay; i++)
        {
            updateTime();
        }
    }
    #endregion
}
