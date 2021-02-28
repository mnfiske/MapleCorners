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
    private void dailyMesses()
    {
		objectPooler.SpawnFromPool("Mess1");
	}



    /*// Update once per frame
	void FixedUpdate()
	{
		objectPooler.SpawnFromPool("Mess1");
	}
	*/
}
