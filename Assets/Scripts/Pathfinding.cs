using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator; //Reference to maze

    public Cell[,] grid;

    public Cell goal;
    public Cell start;

    private void Start()
    {
        SetGrid();
    }

    //Constantly running to find the best path from start to goal (start will be player pos and will change)
    private void Update()
    {
        FindPath();
    }


    private void SetGrid()
    {
        //Initializes Goal and Start
        grid = mazeGenerator.GetGrid();
        goal = grid[29, 30]; //Goal Cell is always at this position (atm)
        start = grid[0, 1]; //Start Cell
    }

    //A* Algorithm that finds best path to goal
    void FindPath()
    {
        List<Cell> unexploredSet = new();
        HashSet<Cell> exploredSet = new();

        unexploredSet.Add(start);
        
        while (unexploredSet.Count > 0)
        {    
            // Select the element with the lowest fCost from the openSet
            Cell currentElement = unexploredSet[0];

            for (int i = 1; i < unexploredSet.Count; i++)
            {
                // Compare current element's fCost with the next element's fCost and hCost
                if (unexploredSet[i].fCost < currentElement.fCost || unexploredSet[i].fCost == currentElement.fCost && unexploredSet[i].hCost < currentElement.hCost)
                {
                    currentElement = unexploredSet[i]; // Updates when new element has better cost 
                }
            }
            //Remove best node from openSet
            unexploredSet.Remove(currentElement);

            //Adds it to the explored set
            exploredSet.Add(currentElement);

            //If goal reached, retrace path
            if (currentElement == goal)
            {
                RetracePath(start, goal);

                return;
            }

            //Checks adjacent cells
            foreach (Cell neighbor in mazeGenerator.GetNeighbors(currentElement))
            {
                //Skips neighbor if its been explored or if its a wall
                if (!neighbor.cellType.Equals(goal.cellType) || exploredSet.Contains(neighbor)) continue;

                //Calculated gcost
                float newMovementCostToNeighbor = currentElement.gCost + GetDistance(currentElement, neighbor);

                //Checks if cheaper path through neighbor
                if (newMovementCostToNeighbor < neighbor.gCost || !unexploredSet.Contains(neighbor))
                {
                    neighbor.gCost = (int)newMovementCostToNeighbor;
                    neighbor.hCost = (int)GetDistance(neighbor, goal);
                    neighbor.parent = currentElement; // Current element becomes parent of neighbor (it leads to it)

                    //Adds to unexplored if it hasn't been explored
                    if(!unexploredSet.Contains(neighbor))
                    {
                        unexploredSet.Add(neighbor);
                    }
                }
            }
        }
    }

    //Gets Manhattan distance between two points with weights against diagonal movement
    float GetDistance(Cell cellA, Cell cellB)
    {
        float distanceX = Mathf.Abs(cellA.position.x - cellB.position.x);
        float distanceY = Mathf.Abs(cellA.position.y - cellB.position.y);

        if (distanceX > distanceY) { 
            return 14*distanceY + 10 * (distanceX - distanceY);
        }
        return 14*distanceX + 10 * (distanceY - distanceX);
    }

    //Retraces best path from end to start
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