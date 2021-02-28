using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessSpawner : SingletonMonoBehavior<MessSpawner>
{
    MesObjectPooler objectPooler;
    void Start()
    {
        objectPooler = MesObjectPooler.Instance;
    }

    // Register to time event system - once per day generate messes
    private void OnEnable()
    {
        EventHandler.AdvanceGameDayEvent += dailyMesses;
    }

    private void OnDisable()
    {
        EventHandler.AdvanceGameDayEvent -= dailyMesses;
    }

    // Make the messes
    private void dailyMesses(int gameYear, Season gameSeason, int gameDay, Weekday gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // be sure queue is full
        objectPooler.RefillPool("Mess1");

        // spawn random number of items
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            objectPooler.SpawnFromPool("Mess1");
        }
    }



    /*// Update once per frame
	void FixedUpdate()
	{
		objectPooler.SpawnFromPool("Mess1");
	}
	*/
}
