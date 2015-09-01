using UnityEngine;
using System.Collections;

public class Wave 
{
	// spawn delay between enemies in a wave 
	public float spawnDelay;

	// counter when next enemy id spawned
	public float nextEnemySpawnEvent = 0;
	public int numberOfEnemies;
	public Vector3 spawnPoint;
		
	public Wave (float spawnDelay, int numberOfEnemies, Vector3 spawnPoint)
	{
		this.spawnDelay = spawnDelay;
		this.numberOfEnemies = numberOfEnemies;
		this.spawnPoint = spawnPoint;
	}
}
