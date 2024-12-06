using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum CellType
{
	path,
	wall,
}
public class Cell
{
	public Vector2 position;
	public CellType cellType;
	public Cell(Vector2 position)
	{
		this.position = position;

		cellType = CellType.wall;
	}
}
public class MazeGenerator : MonoBehaviour
{
	public int width = 21; // Must be odd for proper maze generation
	public int height = 21; // Must be odd for proper maze generation
	[SerializeField] private GameObject cellPrefab;
	private Cell[,] grid;
	[SerializeField] private float cellSize;

	private Vector2Int[] directions = new Vector2Int[]
	{
		new Vector2Int(0, 2),  // Up
		new Vector2Int(0, -2), // Down
		new Vector2Int(2, 0),  // Right
		new Vector2Int(-2, 0)  // Left
	};

	void Start()
	{
		GenerateMaze();
		DrawMaze();
	}

	void GenerateMaze()
	{
		// Initialize the grid with walls
		grid = new Cell[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				grid[x,y] = new Cell(new Vector2(x * cellSize, y * cellSize));
				
			}
		}
		

		// Start the maze generation from a random odd cell
		
		Vector2Int start = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
		if (start.x % 2 == 0) start.x += 1;
		if (start.y % 2 == 0) start.y += 1;
		

		// Perform DFS
		DFS(start);
		
		//choose the first cell from the left to be an opening
		for(int x = 0; x < width; x++){
			if(grid[x, 1].cellType == CellType.path){
				grid[x,0].cellType = CellType.path;
				break;
			}
		}
		
		//choose the first cell from the right on the last column to be an opening
		for(int x = width-1; x >= 0; x--){
			if(grid[x, height-2].cellType == CellType.path){
				grid[x,height-1].cellType = CellType.path;
				break;
			}
		}
	}

	void DFS(Vector2Int current)
	{
		grid[current.x, current.y].cellType = CellType.path; // Mark the current cell as part of the maze

		// Shuffle directions for randomness
		ShuffleDirections();

		foreach (var dir in directions)
		{
			Vector2Int neighbor = current + dir;

			// Check if the neighbor is within bounds and unvisited
			if (neighbor.x > 0 && neighbor.x < width - 1 && neighbor.y > 0 && neighbor.y < height - 1)
			{
				if (grid[neighbor.x, neighbor.y].cellType == CellType.wall)
				{
					// Break the wall between current and neighbor
					Vector2Int wall = current + dir / 2;
					grid[wall.x, wall.y].cellType = CellType.path;

					DFS(neighbor);//The algorithm continues backtracking to previously visited cells in the recursion stack until it completes the maze generation.
				}
			}
		}
	}

	void ShuffleDirections()
	{
		for (int i = 0; i < directions.Length; i++)
		{
			Vector2Int temp = directions[i];
			int randomIndex = Random.Range(i, directions.Length);
			directions[i] = directions[randomIndex];
			directions[randomIndex] = temp;
		}
	}

	void DrawMaze()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (grid[x, y].cellType == CellType.wall) // Draw walls
				{	
					Instantiate(cellPrefab, new Vector2(x * cellSize, y * cellSize), Quaternion.identity);

				}
			}
		}
	}
}
