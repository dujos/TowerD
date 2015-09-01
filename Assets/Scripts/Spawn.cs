using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawn : MonoBehaviour 
{
	// Spawn waves of enemies

	private GameObject target;					// enemy target - Building
	public GameObject enemyPrefab;				// enemy to spaw
	public int spawnCounter;					// keep track of all spawn waves
	public int enemyCounter;					// keep track of all spawn enemies

	public int maxSpawnWaves;					// maximum waves we can spawn

	public float startWaveWait;					// time before spawn waves start
	public float waveWait;						// time between spawn waves
	public float enemySpawnwait;				// time between two enwmies spawn

	public int minNumberEnemies;				//	
	public int maxNumberEnemies;				// 

	public GameObject scorePrefab;				// keep track of all scoring activities

	void Start () 
	{
		StartCoroutine ("SpawnWaveCo");
	}

	public IEnumerator SpawnWaveCo () 
	{
		yield return new WaitForSeconds (startWaveWait);

		while (true) 
		{
			int len = SpawnedEnemies ();
			List<GameObject> buildings = GameObject.FindGameObjectsWithTag ("Building").ToList ();
			GameObject target = TargetSelection (buildings);

			// create enemies
			for (int i = 0; i < len; i++) 
			{
				// we look at direction of our target
				GameObject enemy = (GameObject)Instantiate (enemyPrefab, transform.position, 
					Quaternion.LookRotation (target.transform.position));

				// we set a reference to our target
				// TO DO
				//enemy.GetComponent<Enemy> ().target = target as Building;

				yield return new WaitForSeconds (enemySpawnwait);
			}

			// keep track of all waves and enemies
			spawnCounter++;
			enemyCounter++;

			//score.enemiesSpawned += enemyCounter;
			//score.spawnedWaves += spawnCounter;

			// stop coroutine
			if (maxSpawnWaves == spawnCounter) 
			{
				yield break;
			}

			yield return new WaitForSeconds (waveWait);
		}
	}

	// select target for attack and get position of static target
	private GameObject TargetSelection (List<GameObject> buildings) 
	{
		return buildings.ElementAt (Random.Range (0, buildings.Count));
	}

	// how many enemies must we spawn in one wave
	private int SpawnedEnemies () 
	{
		return Random.Range (minNumberEnemies, maxNumberEnemies);
	}

	// stop spawning enemies
	public void StopSpawnWave () 
	{
		StopCoroutine (SpawnWaveCo ());
	}
}