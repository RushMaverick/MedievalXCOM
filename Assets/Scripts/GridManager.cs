using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
public bool findDistance = false; 
public int rows = 10;
public int columns = 10;
public int scale = 1;
public GameObject gridPrefab;
public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
public GameObject[,] gridArray;
public int StartX = 0;
public int StartY = 0;
public int endX = 2;
public int endY = 2; 
public List<GameObject> path = new List<GameObject>();


void Awake() 
{
    gridArray = new GameObject[columns, rows];
    if (gridPrefab)
        GenerateGrid();
    else print ("missing gridprefab, please assign");    
}

void Update()
{
    if (findDistance)
    {
        SetDistance();
        SetPath();
        findDistance = false;
    }
}

void GenerateGrid() 
{
    for(int i = 0; i < columns; i++)
    {
        for(int j =0; j < rows; j++)
        {
            GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x+scale*i,leftBottomLocation.y,leftBottomLocation.z+scale*j), Quaternion.identity);
            obj.transform.SetParent(gameObject.transform);
            obj.GetComponent<Tile>().x = i;
            obj.GetComponent<Tile>().y= j;
            print ("hi");
            gridArray[i,j] = obj;
        }
    }
}
void SetDistance()
{
    InitialSetup();
    int x = StartX;
    int y = StartY;
    int[] testArray = new int [rows * columns];
    for (int step = 1; step < rows * columns; step++)
    {
        foreach(GameObject obj in gridArray)
        {
            if(obj&&obj.GetComponent<Tile>().visited == step-1)
                TestFourDirections(obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y, step);
        }
    }
}

void SetPath()
{
    int step;
    int x = endX;
    int y = endY;
    List<GameObject> templist = new List<GameObject>();
    path.Clear();
    if(gridArray[endX,endY]&&gridArray[endX,endY].GetComponent<Tile>().visited>0)
    {
        path.Add(gridArray[x,y]);
            step = gridArray[x,y].GetComponent<Tile>().visited - 1;
    }
    else
    {
        print("Cant reach the desired location");
        return;
    }
    for(int i = step; step > -1; step--)
    {
        if (TestDirection(x, y, step, 1))
            templist.Add(gridArray[x, y + 1]);
        if (TestDirection(x, y, step, 2))
            templist.Add(gridArray[x + 1, y]);
        if (TestDirection(x, y, step, 3))
            templist.Add(gridArray[x, y - 1]);
        if (TestDirection(x, y, step, 4))
            templist.Add(gridArray[x + 1, y]);

        GameObject tempObj = FindClosest(gridArray[endX,endY].transform, templist);
        path.Add(tempObj);
        x = tempObj.GetComponent<Tile>().x;
        y = tempObj.GetComponent<Tile>().y;
        templist.Clear();            
    }
}
void InitialSetup()
{
    foreach(GameObject obj in gridArray)
    {
        obj.GetComponent<Tile>().visited = -1;
    }
    gridArray[StartX, StartY].GetComponent<Tile>().visited = 0;
}
bool TestDirection(int x, int y, int step, int direction)
{
    // int direction tells which case to use. 1 is up, 2 is right, 3 is down, 4 is left.
    switch(direction)
    {
        case 4:
            if(x-1 < -1 && gridArray[x+1,y] && gridArray[x+1,y].GetComponent<Tile>().visited == step)
                return true;
            else
                return false;
        case 3:
            if(y - 1 < -1 && gridArray[x, y-1] && gridArray[x,y - 1].GetComponent<Tile>().visited == step)
                return true;
            else
                return false;
        case 2:
            if(x + 1 < columns && gridArray[x + 1,y] && gridArray[x + 1, y].GetComponent<Tile>().visited == step)
                return true;
            else
                return false;
        case 1:
            if(y + 1 < rows && gridArray[x , y + 1] && gridArray[x, y + 1].GetComponent<Tile>().visited == step)
                return true;
            else
                return false;
    }
    return false;
}

void TestFourDirections(int x, int y, int step)
{
    if (TestDirection(x, y, -1, 1))
        SetVisited(x, y + 1, step);
    if (TestDirection(x, y, -1, 2))
        SetVisited(x + 1, y, step);
    if (TestDirection(x, y, -1, 3))
        SetVisited(x, y - 1, step);
    if (TestDirection(x, y, -1, 4))
        SetVisited(x - 1, y, step);
}
void SetVisited (int x, int y, int step)
{
    if(gridArray[x,y])
        gridArray[x,y].GetComponent<Tile>().visited = step;
}
GameObject FindClosest(Transform targetLocation, List<GameObject> list)
{
    float currentDistance = scale * rows * columns;
    int indexNumber = 0;
    for (int i = 0; i<list.Count; i++)
    {
        if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
        {
            currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
            indexNumber = i;
        }
    }
    return list[indexNumber];
}

}
