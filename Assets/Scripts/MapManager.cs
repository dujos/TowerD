using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class MapManager : MonoBehaviour 
{
	// Prefabs
	public GameObject enemyPrefab;
	public GameObject timerUI;

	// enemy spawn points
	public List<GameObject> spawnPoints;
	private EntitiesManager entitiesManager;

	public int minEnemiesPerWave;
	public int maxEnemiesPerWave;
	
	// Wave control attributes
	public List<Wave> nextWaves = new List<Wave> ();
	public List<Wave> currentWaves = new List<Wave> ();

	public float waveSpawnDelay = 24f;
	public float enemySpawnDelay = 1f;
	private float nextWaveSpawnEvent;

	// counter to keep track of all created waves
	private int waveCount = 0;
	public int nextWaveCountDown;
	public int numberOfWaves = 20;

	void Awake()
	{
		entitiesManager = EntitiesManager.Instance ();

		LoadBuildings ();
		MakeWaves ();
	}

	private void LoadBuildings () 
	{
		// load all buildings in scene
		List<GameObject> temp = GameObject.FindGameObjectsWithTag ("Building").ToList ();
		for (int i = 0; i < temp.Count; i++) 
		{
			entitiesManager.AddBuilding (temp[i].GetComponent<Building> ());
		}
	}

	// Create all waves
	private void MakeWaves ()
	{
		Wave wave;
		for (int i = 0; i < numberOfWaves; i++)
		{
			// determine random number of enemies in a single wave
			int spawnedEnemiesInWave = Random.Range (minEnemiesPerWave, maxEnemiesPerWave);
			// determine spawn point of enemies in a single wave
			Vector3 spawnPoint = spawnPoints[Random.Range (0, spawnPoints.Count)].transform.position;

			wave = new Wave (enemySpawnDelay, spawnedEnemiesInWave, spawnPoint);
			nextWaves.Add (wave);
		}
	}

	public void Update ()
	{
		nextWaveCountDown = (int)(nextWaveSpawnEvent - Time.time);
		CreateEnemies ();

		timerUI.GetComponent <Text> ().text = nextWaveCountDown.ToString ();
	}

	// Start a wave from the next Waves list.
	// End game when all the waves have ran
	// all the buildings are dead
	public void SpawnWave ()
	{
		if (nextWaves.Count > 0) 
		{
			Wave wave = nextWaves [0];
			nextWaves.RemoveAt (0);
			currentWaves.Add (wave);
			waveCount++;
		}
	}
	
	// Spawn enemies.
	public void CreateEnemies ()
	{
		if (Time.time >= nextWaveSpawnEvent) 
		{
			nextWaveSpawnEvent = Time.time + waveSpawnDelay;
			SpawnWave ();
		}

		foreach (Wave wave in currentWaves) 
		{
			if (Time.time >= wave.nextEnemySpawnEvent && wave.numberOfEnemies > 0) 
			{
				if (entitiesManager.buildings.Count > 0) 
				{
					int index = Random.Range (0, entitiesManager.buildings.Count);
					Building building = entitiesManager.buildings.ElementAt (index);

					GameObject enemy = (GameObject) Instantiate (enemyPrefab, wave.spawnPoint, 
						Quaternion.LookRotation (building.transform.position));

					// we set the building to attack
					enemy.GetComponent<Enemy> ().target = building;

					wave.numberOfEnemies--;
					nextWaveCountDown = (int)(nextWaveSpawnEvent - Time.time);
					wave.nextEnemySpawnEvent = Time.time + wave.spawnDelay;


				}
			}
		}
		currentWaves.RemoveAll (a => a.numberOfEnemies <= 0);
	}
}