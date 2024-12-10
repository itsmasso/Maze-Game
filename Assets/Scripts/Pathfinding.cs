using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator;

    public Cell[,] grid;

    public Cell goal;
    public Cell start;

    private void Start()
    {
        SetGrid();
    }
    private void Update()
    {
        FindPath();
    }

    private void SetGrid()
    {
        grid = mazeGenerator.GetGrid();
        goal = grid[29, 30]; //Goal Cell is always at this position, maze gen randomizes wall path
        start = grid[0, 1]; //Start Cell
    }

    void FindPath()
    {
        List<Cell> openSet = new();
        HashSet<Cell> closeSet = new();

        openSet.Add(start);
        
        while (openSet.Count > 0)
        {
            Cell currentElement = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentElement.fCost || openSet[i].fCost == currentElement.fCost && openSet[i].hCost < currentElement.hCost)
                {
                    currentElement = openSet[i];
                }
            }
            openSet.Remove(currentElement);
            closeSet.Add(currentElement);

            if (currentElement == goal)
            {
                RetracePath(start, goal);

                return;
            }

            foreach (Cell neighbor in mazeGenerator.GetNeighbors(currentElement))
            {
                if (!neighbor.cellType.Equals(goal.cellType) || closeSet.Contains(neighbor)) continue;

                float newMovementCostToNeighbor = currentElement.gCost + GetDistance(currentElement, neighbor);

                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = (int)newMovementCostToNeighbor;
                    neighbor.hCost = (int)GetDistance(neighbor, goal);
                    neighbor.parent = currentElement;

                    if(!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    float GetDistance(Cell cellA, Cell cellB)
    {
        float distanceX = Mathf.Abs(cellA.position.x - cellB.position.x);
        float distanceY = Mathf.Abs(cellA.position.y - cellB.position.y);

        if (distanceX > distanceY) { 
            return 14*distanceY + 10 * (distanceX - distanceY);
        }
        return 14*distanceX + 10 * (distanceY - distanceX);
    }

    void RetracePath(Cell start,  Cell end)
    {
        List<Cell> path = new();

        Cell curr = end;

        while (curr != start) {
            path.Add(curr);
            curr = curr.parent;
        }

        path.Reverse();
        mazeGenerator.path = path;
    }
}