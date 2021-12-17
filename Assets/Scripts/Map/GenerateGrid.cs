using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateGrid : MonoBehaviour
{
    public GameObject ground;
    public GameObject unbrakableWall;
    public GameObject brakableWall;
    
    private int colNum = 16;
    private int lineNum = 12;
    
    private List<List<Cell>> grid = null;
    
    private bool onceDraw = false;

    void Start()
    {
        DrawMap();
        GenerateBrekableWall();
    }

    void DrawMap()
    {
        if(onceDraw){return;}

        onceDraw = true; // prevent reinstanciate map
        grid = new List<List<Cell>>();
        
        for (int l = 0; l <= lineNum; l++)
        {
            grid.Add(new List<Cell>());
            for (int c = 0; c <= colNum; c++)
            {
                Vector3 vector3 = new Vector3((float)c, (float)l);
                Instantiate(ground,vector3,Quaternion.identity);

                Cell cell = null;
                
                if (l == lineNum || l == 0 || c == colNum || c == 0)
                {
                    cell = new Cell(col:c,line:l ,unbrakableWall);
                    cell.instanciateGameObject();

                }else if (l%2==0 && c%2==0)
                {
                    
                    cell = new Cell(col:c,line:l ,unbrakableWall);
                    cell.instanciateGameObject();
                }
                
                grid[l].Add(cell);
            }
        }
    }

    void GenerateBrekableWall()
    {
        if(!onceDraw){return;}

        for (int l = 0; l < grid.Count; l++)
        {
            for (int c = 0; c < grid[l].Count; c++)
            {

                if (grid[l][c] == null )
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        Cell cell = null;
                        cell = new Cell(col:c,line:l ,brakableWall);
                        cell.instanciateGameObject();
                    }
                }
                
            }
        }
    }
    void Update()
    {
        
    }
}
