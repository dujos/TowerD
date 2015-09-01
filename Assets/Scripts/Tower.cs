using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof (BoxCollider))]
public class Tower : MonoBehaviour 
{
	public GameObject projectilePrefab;
	public int damage = 10;
	public int fireRange = 5;
	public float reloadTime = 1f;
	private int killCount = 0;

	private List<Enemy> targets;
	private float nextFireEvent;
	private EntitiesManager entitiesManager;

	public float turnSpeed = 1f;
	private Quaternion startRotation;

	public AudioSource shotClip;

	// tower fire effect 
	private ParticleSystem gunFireEffect;

	void Awake()
	{
		entitiesManager = EntitiesManager.Instance ();
		entitiesManager.AddTower (this);
	}

	void Start ()
	{
		// make tower a trigger for easy tracking of enemies
		GetComponent <BoxCollider> ().isTrigger = true;
		targets = new List<Enemy>();

		// remember starting direction
		startRotation = transform.rotation;

		// sound
		shotClip = GetComponent<AudioSource> ();

		// particles
		//gunFireEffect = GetComponentInChildren<ParticleSystem> ();
		//gunFireEffect.Stop ();
	}

	// check for targets in range of tower
	// when enemy enters detection area add it to shootable targets
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{	
			targets.Add (other.GetComponent<Enemy> ());
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other != null && targets.Select (t => t!= null && t.gameObject).Contains(other.gameObject)) 
		{
			targets.Remove (other.GetComponent<Enemy> ());
		}
	}

	void Update ()
	{
		if (targets.Count > 0)
		{
			Enemy selectedTarget = targets.ElementAt (Random.Range (0, targets.Count));
			if (selectedTarget != null) 
			{
				TurnToTarget (selectedTarget.gameObject);

				if (Time.time >= nextFireEvent)
				{
					Fire (selectedTarget);
				}
			}
			else
			{
				TurnReset ();

				nextFireEvent = Time.time + reloadTime;
				targets.Remove(selectedTarget);
			}      
		}
	}

	void Fire (Enemy shootTarget)
	{
		// play sound
		shotClip.Play ();

		Vector3 targetPosition = shootTarget.transform.position;
		nextFireEvent = Time.time + reloadTime;
		GameObject temp = Instantiate (projectilePrefab, transform.position, 
		                               Quaternion.LookRotation (targetPosition)) as GameObject;
		
		Projectile projectile = temp.GetComponent<Projectile> ();
		projectile.damage = damage;
		projectile.target = shootTarget;
		projectile.targetPosition = targetPosition;
		projectile.range = fireRange;
		projectile.Gun = this;

		// rotate particles towards our target
		//gunFireEffect.transform.LookAt (targetPosition);
		//gunFireEffect.Play ();
	}
	
	public void EnemyKilled () 
	{
		killCount++;
	}

	public void TurnToTarget (GameObject target) 
	{
		transform.rotation = Quaternion.Slerp (transform.rotation, 
			Quaternion.LookRotation ((target.transform.position - transform.position).normalized), 
		                                       turnSpeed * Time.deltaTime);
	}
	
	public void TurnReset () 
	{
		transform.rotation = Quaternion.Slerp (transform.rotation, startRotation, turnSpeed * Time.deltaTime);
	}
}