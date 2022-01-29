using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOver : MonoBehaviour
{

    public GameObject UnbreakableWallPrefab;
    
    int[][] timeOverUpPoints = new int[][]
    {
        new int[]{1,0},
        new int[]{2,1},
        new int[]{3,2},
        new int[]{4,3},
        new int[]{5,4},
        new int[]{6,5},
    };
            
    int[][] timeOverRightPoints = new int[][]
    {
        new int[]{1,11},
        new int[]{2,10},
        new int[]{3,9},
        new int[]{4,8},
        new int[]{5,7},
        new int[]{6,6},
    };
            
    int[][] timeOverDownPoints = new int[][]
    {
        new int[]{15,11},
        new int[]{14,10},
        new int[]{13,9},
        new int[]{12,8},
        new int[]{11,7},
        new int[]{10,6},
    };
            
    int[][] timeOverLeftPoints = new int[][]
    {
        new int[]{15,1},
        new int[]{14,2},
        new int[]{13,3},
        new int[]{12,4},
        new int[]{11,5},
        new int[2],
    };
    
    private int posX;
    private int posY;
    private int direction;
    private int round;

    private bool once = false;

    void Update()
    {
        if (GameManager.Instance.currentTime > 0)
        {
            GameManager.Instance.currentTime -= Time.deltaTime;
            if (GameManager.Instance.currentTime <= 0) GameManager.Instance.currentTime = 0;
        }
        
        if (GameManager.Instance.currentTime <= 0 && !once)
        {
            once = true;
            
            posX = 1;
            posY = 0;
            direction = 0;
            round = 0;
            
            InvokeRepeating("drawWall",0f,1f);
        }
    }

    private void drawWall()
    {
        GameManager.Instance.mapCellsLayer[posY][posX].Erase(force: true);
        GameManager.Instance.mapCellsLayer[posY][posX].Draw(UnbreakableWallPrefab, force: true);
        GameManager.Instance.mapCellsLayer[posY][posX].erasable=false;
        
        if(posX==5 && posY==4) CancelInvoke(); // stop instantiate wall
        
        if (posX == timeOverUpPoints[round][0] && posY == timeOverUpPoints[round][1]) direction = 0;
        else if (posX == timeOverRightPoints[round][0] && posY == timeOverRightPoints[round][1]) direction = 1;
        else if (posX == timeOverDownPoints[round][0] && posY == timeOverDownPoints[round][1]) direction = 2;
        else if (posX == timeOverLeftPoints[round][0] && posY == timeOverLeftPoints[round][1])
        {
            direction = 3;
            round++;
        }

        if (direction == 0) posY++; // up
        else if (direction == 1) posX++; // right
        else if (direction == 2) posY--; // down
        else if (direction == 3) posX--; // left
    }

    private void OnDisable()
    {
        CancelInvoke();
        once = false;
    }
}
