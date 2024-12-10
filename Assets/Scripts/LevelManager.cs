using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private MazeGenerator mazeGenerator;
	[SerializeField] private Pathfinding pathfinder;
	[SerializeField] private GameObject player;
	[SerializeField] private float timerDuration = 60f; // Total duration in seconds
	private float timeRemaining;
	[SerializeField] private bool isTimerRunning = false; // Start or stop the timer

	[SerializeField] private TMP_Text timerText; // Drag your TMP Text here
	[SerializeField] private TMP_Text levelText;
	
	void Start()
	{

		mazeGenerator.GenerateMaze();
		mazeGenerator.DrawMaze();
		pathfinder.SetGrid();
		mazeGenerator.DrawPathfindingPath();
		player.transform.position = mazeGenerator.startPosition;
		timeRemaining = timerDuration; // Set initial time
		isTimerRunning = true;
	}
	

	// Update is called once per frame
	private void Update()
	{
		
		levelText.text = string.Format("STREAK {0}", GameManager.instance.streak);
		if (isTimerRunning)
		{
			if (timeRemaining > 0)
			{
				// Countdown
				timeRemaining -= Time.deltaTime;
				UpdateTimerUI();
			}
			else
			{
				// Timer ends
				Debug.Log("Timer Finished!");
				timeRemaining = 0;
				isTimerRunning = false;
				UpdateTimerUI(); // Ensure it shows 00:00 when finished
			}
		}
	}
	
	void UpdateTimerUI()
	{
		timerText.text = string.Format("{0}", Mathf.FloorToInt(timeRemaining));
	}

	
}
