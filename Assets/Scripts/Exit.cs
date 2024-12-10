using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

	private void OnTriggerEnter2D(Collider2D other) {
		
		PlayerMovement playerScript = other.gameObject.GetComponent<PlayerMovement>();

		if(playerScript != null)
		{
			GameManager.instance.IncrementStreak();
			Debug.Log("reloading scnee");
			GameManager.instance.ReloadScene();
		}
	}
	
	 
}
