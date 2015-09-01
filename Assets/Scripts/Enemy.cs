using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour 
{
	public delegate void DieDetector (Enemy enemy);
	public static event DieDetector OnEnemyDie;


	public int damage;							// amount of damage unit can produce
	public int gold = 10;
	public int life;
	private bool active = true;

	private EntitiesManager entitiesManager;
	private NavMeshAgent agent;					// navigation agent
	private List<Building> buildings;			// all buildings in scene
	public Building target;						// we move towards this enemy
	
	// fire particles before destruction
	private ParticleSystem fire;
	public float fireDelay = 3f;

	void Awake ()
	{
		entitiesManager = EntitiesManager.Instance ();
		entitiesManager.AddEnemy (this);

		buildings = new List<Building> ();
		Building.OnBuildingDie += BuildingDestroyReselectTarget;

		// fire particle system
		fire = GetComponentInChildren <ParticleSystem> ();
		fire.Stop ();
	}

	public void Start () 
	{
		LoadNavigationAgent ();
	}

	void Update ()
	{
		if (target == null && active) 
		{
			active = false;
			DestroyEntity ();
		}
	}

	public void LoadNavigationAgent () 
	{
		agent = GetComponent<NavMeshAgent> ();
		if (target != null) 
			agent.SetDestination (target.transform.position);
	}
	
	// select target where we disregard previous target
	public Vector3 SelectTarget (Building previousTarget) 
	{
		buildings = entitiesManager.buildings;
		if (buildings.Count > 0) 
		{
			//(buildings.Find (t => t != null && t.gameObject).Contains(!target.gameObject))
			for (int i = 0; i < buildings.Count; i++) 
			{
				if (buildings[i] != previousTarget) 
				{
					target = buildings[i];
					return target.transform.position;
				}
			}
		}
		target = null;
		return Vector3.zero;
	}

	public void BuildingDestroyReselectTarget (Building previousTarget) 
	{
		//if (agent != null) 
		//	agent.SetDestination (SelectTarget (previousTarget));
	}

	// damage taken from building - instant death
	public void Detonate () 
	{
		life = -1;
		Explode ();
		DestroyEntity ();
	}

	// damage taken from tower
	public void WeaponDamage (int damage)
	{
		life -= damage;
		if (life < 0) 
		{
			Explode ();
			DestroyEntity ();
		}
	}

	public void Explode () 
	{
		// stop pathfinding
		agent.enabled = false;
		agent.speed = 0;

		// run fire particle system before destroying object
		//StartCoroutine ("Fire");
	}

	public IEnumerator Fire () 
	{
		fire.Play ();
		yield return new WaitForSeconds (fireDelay);
	}

	public void DestroyEntity ()
	{
		entitiesManager.RemoveEnemy (this);

		// notify enemy died
		if(OnEnemyDie != null) 
			OnEnemyDie (this);

		Destroy (gameObject);
	}
}