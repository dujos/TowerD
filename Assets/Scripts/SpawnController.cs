using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnController : MonoBehaviour 
{
	public delegate void EnableSpawn ();
	public static event EnableSpawn OnEnableSpawn; 

	public delegate void SpawnWaveCountDetection ();
	public static event SpawnWaveCountDetection OnSpawnWaveCountDetection;

	public delegate void SpawnWaveCountdown (int countdown);
	public static event SpawnWaveCountdown OnSpawnWaveCountdown;

	public List<GameObject> spawnWaves;						// all spawn points

	public float spawnDuration = 10f;						// time between spawn events
	public int spawnWaveCounter;							// track number of spawn events
	private int countdown;									// countdown;
	private int nextWaveSpawnEvent;
	public bool canStart = true;

	public void Awake () 
	{
		// load all buildings
		List<GameObject> buildings = GameObject.FindGameObjectsWithTag ("Building").ToList ();
		for (int i = 0; i < buildings.Count; i++) 
		{
			EntitiesManager.Instance ().AddBuilding (buildings[i].GetComponent<Building> ());
        }
    }

	void Start () 
	{
		StartCoroutine (SpawnWaveUnlimitedCo ());
	}

	public IEnumerator SpawnWaveUnlimitedCo () 
	{

		while (true) 
		{
			// notify a new wave started
			if (OnEnableSpawn != null) 
				OnEnableSpawn ();

			// spawn all active waves
			foreach (GameObject wave in spawnWaves) 
			{
				StartCoroutine (wave.GetComponent<WaveW> ().SpawnWave ());
			}
			
			// track all spawn events
			if (OnSpawnWaveCountDetection != null)
				OnSpawnWaveCountDetection ();

			// wait to launch next wave of enemies
			yield return new WaitForSeconds (spawnDuration);
		}
	}

	public void Update () 
	{
		countdown = (int)(nextWaveSpawnEvent - Time.time);

		// notify counter on ui
		if (OnSpawnWaveCountdown != null)
			OnSpawnWaveCountdown (countdown);

		// reset countdown
		if (countdown <= 0) 
		{
			nextWaveSpawnEvent = (int)(Time.time + spawnDuration);
		}
	}

	/*
	private int DoBuildingsExist () 
	{
	}
	*/
}
