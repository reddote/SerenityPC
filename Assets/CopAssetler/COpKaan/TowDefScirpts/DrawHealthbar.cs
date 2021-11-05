using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Put on gameObjects with healthbars to request drawing healthbar each frame. 
/// </summary>

public class DrawHealthbar : MonoBehaviour
{
	TowDefMonster monster;
	CapsuleCollider collisionCollider;
	Vector3 healthBarDrawOffset;
 
	Vector3 DrawPos
	{
		get
		{
			return transform.position + healthBarDrawOffset;
		}
	}
 
	float FillAmount
	{
		get
		{

			return (float)monster.Health / monster.MaxHealth;
		}
	}
 
	bool IsAlive
	{
		get
		{
			return monster.isAlive;
		}
	}
 
	void Start()
	{
		if(HealthbarDrawer.Instance == null)
		{
			Debug.Log("HealthbarDrawer is null. DrawHealthbar on this object is disabled.");
			enabled = false;
			return; 
		}
 
		// Get healthbar draw offset from collision collider
		foreach(var col in GetComponents<CapsuleCollider>())
		{
			if(!col.isTrigger)
			{
				collisionCollider = col;
				float colliderHeight = collisionCollider.height;
				healthBarDrawOffset = Vector3.up * colliderHeight * transform.localScale.y;
				break;
			}
		}
 
		monster = GetComponent<TowDefMonster>(); 
	}
 
	void Update()
	{
		TryDrawHealthbar();
	}
 
	void TryDrawHealthbar()
	{
		// Draw healthbar unless character is dead 
		if(IsAlive)
		{
			HealthbarDrawer.Instance.DrawHealthbar(collisionCollider, DrawPos, FillAmount);
		}
	}
}