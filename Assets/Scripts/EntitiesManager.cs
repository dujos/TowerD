using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntitiesManager
{
	private static EntitiesManager instance;
	
	public List<Enemy> enemies = new List<Enemy> ();
	public List<Tower> towers = new List<Tower> ();
	public List<Building> buildings = new List<Building> ();
    
	public EntitiesManager ()
	{
		EntitiesManager.instance = this;
	}

	public static EntitiesManager Instance ()
	{
		if (EntitiesManager.instance != null) 
		{
			return EntitiesManager.instance;
		}
		return new EntitiesManager ();
    }

	public void AddEnemy (Enemy entity) 
	{
		enemies.Add (entity);
	}
	
	public void AddTower (Tower tower) 
	{
		towers.Add (tower);
	}

	public void AddBuilding (Building building) 
	{
		if (!buildings.Contains (building)) 
		{
			buildings.Add (building);
		}
	}

	public void RemoveEnemy (Enemy entity)
	{
		if (enemies.Contains (entity))
		{
			enemies.Remove (entity);
		}
	}

	public void RemoveBuilding (Building building) 
	{
		if (buildings.Contains (building))
		{
			buildings.Remove (building);
		}
	}
}