## Pathfinding.cs
File responsible for the A* Algorithm Code.

Open Source Contribution: https://www.youtube.com/watch?v=mZfyt03LDH4

### SetGrid Method: 
-  Initializes goal and start positions

### FindPath Method:
-  Method responsible for the actual algorithm
-  Adds start position to the unexplored set
-  Starts checking every unexplored item in the set:
    -  If the fcost of that item is less than the current item OR If the fcost is the same but the hcost is less than the current item
        -   Replaces current item with new item
-  Removes current item from explored set and adds it to explored set
-  Checks if goal was reached
    -  Call Retrace Method
    -  Return function
-  Checks neighbors:
    -  Returns if neighbor is a wall or if its been explored
    -  Calculates gcost of neighbor through GetDistance Method and checks if its less or if it hasn't been explored
        -  If true, current item becomes parent of neighbor
        -  Adds to unexplored set if it hasnt been explored

### GetDistance Method:
-  Calculates Manhattan Distance between points with weights for diagonal and horizontal/vertical movement

### Retrace Method:
-  Uses backtracking to get to start from the goal cell using the parent of each cell (linked list like)
-  Calls DrawPath with path as a parameter

### DrawPath Method:
- For each item in the path, creates a Path object that is instantiated into the scene to show the path to the goal
