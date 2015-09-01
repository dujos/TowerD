using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveW : MonoBehaviour
{
	public delegate void EnableSpawn ();
	public static event EnableSpawn OnEnableSpawn;


	public GameObject enemyPrefab;			// spawned enemy
	public int maxEnemies;
	public int minEnemies;
	public float enemySpawnDelay;			// delay between enemy spawn

	private EntitiesManager instance;

	public void Awake () 
	{
		instance = EntitiesManager.Instance ();
	}

	public IEnumerator SpawnWave () 
	{
		// enemy destination - random selection
		Building building = instance.buildings.ElementAt (Random.Range (0, instance.buildings.Count));

		// number of enemies in a wave - random selection
		int enemyCount = Random.Range (minEnemies, maxEnemies);
		while (enemyCount > 0) 
		{
			GameObject enemy = (GameObject)Instantiate (enemyPrefab, transform.position, transform.rotation);
			enemy.GetComponent<Enemy> ().target = building;
			enemyCount--;

			// spawn delay
			yield return new WaitForSeconds (enemySpawnDelay);
		}
		yield return null;
	}
}