using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerTile : MonoBehaviour 
{
	public GameObject redTower;
	public GameObject ui;

	private int towerLayer;
	private int tileLayer;


	public void Start () 
	{
		towerLayer = LayerMask.GetMask ("Tower");
		tileLayer = LayerMask.GetMask ("Tile");
	}
	
	public void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, tileLayer))
		{
			if (!Physics.Raycast (ray, out hit, Mathf.Infinity, towerLayer)) 
			{
				// check gold balance
				UIController gold = ui.GetComponent <UIController> ();
				if (Input.GetMouseButton (0) ) 
				{
					Debug.Log (gold.GoldBalance ());
					// decrease our gold holdings for one unit
					gold.GoldCountAmount (-1);
					BuildTower (hit.point);
				}
			}
		}
	}

	public void BuildTower (Vector3 position) 
	{
		Instantiate (redTower, position, transform.rotation);
	}
}