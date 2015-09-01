using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed;
	public float range;
	public int damage;

	public Enemy target;
	public Vector3 targetPosition;
	public Tower Gun { get; set; }

	private float distance;
	private EntitiesManager entitiesManager;
	
	void Awake ()
	{
		distance = 0;
		entitiesManager = EntitiesManager.Instance ();
	}
	
	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);

		distance += Time.deltaTime * speed;

		if (distance > range || Vector3.Distance (transform.position, targetPosition) < 0.1f) 
		{
			// destroy projectile
			Destroy (gameObject);
			if (target != null) 
			{
				// tell tower he killed an enemy
				Gun.EnemyKilled ();
				// enemy takes damage
				target.WeaponDamage (damage);
			}
		}
	}
}