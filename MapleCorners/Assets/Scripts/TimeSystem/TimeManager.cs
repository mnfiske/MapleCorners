// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonoBehavior<TimeManager>
{
  //Track the game's time properties
  private int day = 1;
  private int hour = 6;
  private int minute = 30;
  private int second = 0;
  private Weekday weekday = Weekday.Mon;
  private bool clockPaused = false;
  private float tick = 0f;

  //Years and seasons are slated to be added in an upcoming release. Add properies here so the methods
  //can be stubbed out to reduce later refactoring
  private int year = 1;
  private Season season = Season.Spring;

  /// <summary>
  /// Call the AdvanceGameMinuteEvent as this component initializes itself
  /// </summary>
  private void Start()
  {
    EventHandler.CallAdvanceGameMinuteEvent(year, season, day, weekday, hour, minute, second);
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
