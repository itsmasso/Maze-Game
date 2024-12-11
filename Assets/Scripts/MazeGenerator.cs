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

	public int gCost; //Cost from start to current
	public int hCost; //Heuristic Cost

	public Cell parent; //Used for backtrace

	public bool isPitStop;
	public Vector2 pitStopDirection;
	public Cell(Vector2 position)
	{
		this.position = position;
		isPitStop = false;
		cellType = CellType.wall;
	}

	//Total Cost
    public int fCost 
	{
		get
		{
			return gCost + hCost;
		}
	}
}

}
public class MazeGenerator : MonoBehaviour
{
	[SerializeField] private int width = 21; // Must be odd for proper maze generation
	[SerializeField] private int height = 21; // Must be odd for proper maze generation
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject pathPrefab;
	[SerializeField] private GameObject exitPrefab;
	public Cell[,] grid {get; private set;}
	public float cellSize;
	public Transform mazeParentObj;
	public List<Cell> path = new List<Cell>();
	public Vector2 startPosition {get; private set;}
	public Vector2Int end;
	public Vector2Int start;
	

	private Vector2Int[] directions = new Vector2Int[]
	{
		new Vector2Int(0, 2),  // Up
		new Vector2Int(0, -2), // Down
		new Vector2Int(2, 0),  // Right
		new Vector2Int(-2, 0)  // Left
	};

    void Awake()
	{
		GenerateMaze();
		DrawMaze();
	}

	public void GenerateMaze()
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
		

		//Start the maze generation from a random odd cell
		start = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));

		if (start.x % 2 == 0) start.x += 1;
		if (start.y % 2 == 0) start.y += 1;
		startPosition = new Vector2(start.x * cellSize + mazeParentObj.position.x, start.y * cellSize + mazeParentObj.position.y);

		//Perform DFS
		DFS(start);
		
		do
		{
			end = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
			if (end.x % 2 == 0) end.x += 1;
			if (end.y % 2 == 0) end.y += 1;
		} while (end == start); //Ensure the end isn't the same as the start
		
		//spawn the exit prefab
		Instantiate(exitPrefab, new Vector2(end.x * cellSize + mazeParentObj.position.x, end.y * cellSize + mazeParentObj.position.y), Quaternion.identity);
		
		//determine which cells are supposed to be pitstops for the player to stop on
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (grid[x, y].cellType == CellType.path) 
				{	
					Cell currentCell = grid[x, y];
					
					//Neighbor checks
					bool leftIsWall = x > 0 && grid[x - 1, y].cellType == CellType.wall;
					bool rightIsWall = x < width - 1 && grid[x + 1, y].cellType == CellType.wall;
					bool topIsWall = y < height - 1 && grid[x, y + 1].cellType == CellType.wall;
					bool bottomIsWall = y > 0 && grid[x, y - 1].cellType == CellType.wall;


					bool isHorizontalPitStop = leftIsWall && rightIsWall && !topIsWall && !bottomIsWall;
					bool isVerticalPitStop = topIsWall && bottomIsWall && !leftIsWall && !rightIsWall;

					if (!isHorizontalPitStop && !isVerticalPitStop)
					{
						currentCell.isPitStop = true;
					}
							
				}
			}
		}
		
	}
    public List<Cell> GetNeighbors(Cell cell)
    {
		List<Cell> neighbors = new();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0) continue;

				float checkX = cell.position.x + x;
                float checkY = cell.position.y + y;

				if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
				{
					neighbors.Add(grid[(int)checkX, (int)checkY]);
				}

            }
        }

		return neighbors;
    }

	public List<Cell> path;

	private bool CheckIfInBounds(Vector2Int cell)
	{
		return cell.x > 0 && cell.x < width - 1 && cell.y > 0 && cell.y < height - 1;

	}
	
	

	private void DFS(Vector2Int current)
	{
		grid[current.x, current.y].cellType = CellType.path; //Mark the current cell as part of the maze

		//Shuffle directions for randomness
		ShuffleDirections();

		foreach (Vector2Int dir in directions)
		{
			Vector2Int neighbor = current + dir;

			//Check if the neighbor is within bounds and unvisited
			if (CheckIfInBounds(neighbor))
			{
				if (grid[neighbor.x, neighbor.y].cellType == CellType.wall)
				{
					//Break the wall between current and neighbor
					Vector2Int wall = current + dir / 2;
					grid[wall.x, wall.y].cellType = CellType.path;

					DFS(neighbor);//The algorithm continues backtracking to previously visited cells in the recursion stack until it completes the maze generation.
				}
			}
		}
	}
    public Cell[,] GetGrid()
    {
        return grid;  // Exposes the grid
    }


	private void ShuffleDirections()
	{
		for (int i = 0; i < directions.Length; i++)
		{
			Vector2Int temp = directions[i];
			int randomIndex = Random.Range(i, directions.Length);
			directions[i] = directions[randomIndex];
			directions[randomIndex] = temp;
		}
	}
	

	public void DrawMaze()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
                
        
                if (grid[x, y].cellType == CellType.wall) // Draw walls
				{	

					GameObject wall = Instantiate(wallPrefab, new Vector2(x * cellSize + mazeParentObj.position.x, y * cellSize + mazeParentObj.position.y), Quaternion.identity);
					
					wall.transform.parent = mazeParentObj.transform;

				}
				else
				{
					GameObject path = Instantiate(pathPrefab, new Vector2(x * cellSize + mazeParentObj.position.x, y * cellSize + mazeParentObj.position.y), Quaternion.identity);
					path.transform.parent = mazeParentObj.transform;
					if(grid[x, y].isPitStop)
					{
						path.GetComponent<PathCell>().ChangeToPitstop();
					}
					
					
				}
			}
		}
	}
}
