using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Building : MonoBehaviour 
{
	public delegate void DieDetector (Building building);	// listener when building is destroyed
	public static event DieDetector OnBuildingDie;

	public int gold;
	public int life;

	private EntitiesManager entitiesManager;

	// fire effect on death
	private ParticleSystem fire;
	public float fireDelay = 1.5f;


	public void Awake () 
	{
		// make building a trigger
		GetComponent<BoxCollider> ().isTrigger = true;
		
		entitiesManager = EntitiesManager.Instance ();
		entitiesManager.AddBuilding (this);

		// on building die
		fire = GetComponentInChildren <ParticleSystem> ();
		fire.Stop ();
	}

	public void Start () {}

	void Update () {}

	// when enemy enters building area detonate it
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{
			// give damage to building
			AttackBuilding (other.GetComponent<Enemy> ().damage);
			// give damage to enemy
			other.GetComponent<Enemy> ().Detonate ();
		}
	}

	public void AttackBuilding (int damage) 
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
		// run fire particle system before destroying object
		StartCoroutine ("Fire");
	}

	public IEnumerator Fire () 
	{
		fire.Play ();
		yield return new WaitForSeconds (fireDelay);
	}

	public void DestroyEntity ()
	{
		entitiesManager.RemoveBuilding (this);

		// notify building died
		if(OnBuildingDie != null) 
			OnBuildingDie (this);

		Destroy (gameObject, fireDelay);
	}
}