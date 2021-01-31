// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI timeText = null;
  [SerializeField] private TextMeshProUGUI dateText = null;
  [SerializeField] private TextMeshProUGUI seasonText = null;
  [SerializeField] private TextMeshProUGUI yearText = null;

  /// <summary>
  /// Subscribe to AdvanceGameMinuteEvent
  /// </summary>
  private void OnEnable()
  {
    EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
  }

  /// <summary>
  /// Unsubscribe from AdvanceGameMinuteEvent
  /// </summary>
  private void OnDisable()
  {
    EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="year"></param>
  /// <param name="season"></param>
  /// <param name="day"></param>
  /// <param name="weekday"></param>
  /// <param name="hour"></param>
  /// <param name="minute"></param>
  /// <param name="second"></param>
  private void UpdateGameTime(int year, Season season, int day, Weekday weekday, int hour, int minute, int second)
  {
    string period = string.Empty;
    string minuteText = string.Empty;
    string hourText = string.Empty;

    //Only display the time in ten minute increments
    minute = minute - (minute % 10);
    if (minute < 10)
    {
      minute = 0;
    }

    //If it's the afternoon/evening, set the period to pm and get the correct 12 clock time
    if (hour >= 12)
    {
      if (hour >= 13)
      {
        hour -= 12;
      }

      period = " pm";
    }
    //It it's the morning, just set the period
    else
    {
      period = " am";
    }

    //Ensure the minute always displays as two digits
    if (minute < 10)
    {
      minuteText = "0" + minute.ToString();
    }
    else
    {
      minuteText = minute.ToString();
    }

    //Ensure the hour always displays as two digits
    if (hour < 10)
    {
      hourText = "0" + hour.ToString();
    }
    else
    {
      hourText = hour.ToString();
    }

    //Build the time string
    timeText.SetText( hourText + " : " + minuteText + period );
    //Build day string
    dateText.SetText(System.Enum.GetName(weekday.GetType(), weekday) + ". " + day.ToString());
    //TODO: When implementing seasons and years, change these placeholders to reflect those values
    seasonText.SetText("Day");
    yearText.SetText("Time");
  }
}
