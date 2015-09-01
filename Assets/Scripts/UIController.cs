using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
	public Text spawnTimerText;
	public Text killCountText;
	public Text goldCountText;
	public Text buildingCountText;
	public Text spawnStartText;
	public Text spawnCountText;

	private int killCount;					// number of enemies killed;
	public int goldCount;					// amount of gold we have
	private int buildingCount;				// player health
	private int spawnCounter;				// track all spawn events
	private int spawnLaunchMessage;			// notify of spawn launch

	public Button redTower;
	public Button greenTower;

	void Start () 
	{
		killCount = 0;
		buildingCount = EntitiesManager.Instance ().buildings.Count;
		Debug.Log (EntitiesManager.Instance ().buildings.Count);

		killCountText.text = killCount.ToString ();
		buildingCountText.text = buildingCount.ToString ();
		goldCountText.text = goldCount.ToString ();
	}

	public void OnEnable () 
	{
		Enemy.OnEnemyDie += EnemyDie;
		Building.OnBuildingDie += BuildingDie;
		SpawnController.OnEnableSpawn += SpawnStart;
		SpawnController.OnSpawnWaveCountDetection += SpawnLaunched;
		SpawnController.OnSpawnWaveCountdown += CountDown;
	}

	public void OnDisable () 
	{
		Enemy.OnEnemyDie -= EnemyDie;
		Building.OnBuildingDie -= BuildingDie;
		SpawnController.OnEnableSpawn -= SpawnStart;
		SpawnController.OnSpawnWaveCountDetection -= SpawnLaunched;
		SpawnController.OnSpawnWaveCountdown -= CountDown;
	}

	public void SpawnStart () 
	{
		StartCoroutine (FlashMessageCo ());
	}

	public void CountDown (int count) 
	{
		spawnTimerText.text = count.ToString ();
	}

	public IEnumerator FlashMessageCo () 
	{
		spawnStartText.enabled = true;
		spawnStartText.text = "Get Ready!";
		yield return new WaitForSeconds (3);
		spawnStartText.enabled = false;
		yield return null;
	}

	public void SpawnLaunched () 
	{
		spawnCounter++;
		spawnCountText.text = spawnCounter.ToString ();
	}

	// update existing buildings
	public void BuildingDie (Building building) 
	{
		// subtract gold amount
		int temp =- building.gold;
		GoldCountAmount (temp);

		// update building counter
		BuildingCountAmount ();
	}

	// update enemy kill counter
	public void EnemyDie (Enemy enemy) 
	{
		// update kill amount
		KillCountAmount ();
		// add gold amount
		GoldCountAmount (enemy.gold);
	}

	public void BuildingCountAmount () 
	{
		buildingCount = Mathf.Max (0, buildingCount--);
		buildingCountText.text = buildingCount.ToString ();
	}

	// update enemy kill amount
	public void KillCountAmount () 
	{
		killCount++;
		killCountText.text = killCount.ToString ();
	}

	// update gold amount
	public void GoldCountAmount (int coin) 
	{
		goldCount += coin;
		goldCount = Mathf.Max (0, goldCount);
		goldCountText.text = goldCount.ToString ();
	}

	public int GoldBalance () 
	{
		return goldCount;
	}
}