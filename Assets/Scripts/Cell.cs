using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Cell
{

    public static int[][] ForbiddenDrawPositions = new int[][]
        {
            new int[] { 1, 11 },
            new int[] { 2, 11 },
            new int[] { 1, 10 },
            new int[] { 15, 1 },
            new int[] { 14, 1 },
            new int[] { 15, 2 },
            new int[] { 15, 11 },
            new int[] { 14, 11 },
            new int[] { 15, 10 },
            new int[] { 1, 1 },
            new int[] { 1, 2 },
            new int[] { 2, 1 },
        };

    private GameObject gameObject = null;
    private int col = -1;
    private int row = -1;
    private bool breakable = true;
    private bool isForbiddenDrawPositions = false;

    public Cell(int col, int row, GameObject gameObject){
        this.gameObject = gameObject;
        this.row = row;
        this.col = col;
    }

    public Cell(int col, int row){
        this.row = row;
        this.col = col;
    }

    public void Draw(bool force = false)
    {

        if (!force)
        {
            foreach (int[] position in ForbiddenDrawPositions)
            {
                if (this.col == position[0] && this.row == position[1]) return;
            }
        }

        if (this.gameObject != null) MonoBehaviour.Instantiate(this.gameObject, new Vector3((float)col, (float)row), this.gameObject.transform.rotation);
    }

    public void SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void SetBreakable(bool breakable)
    {
        this.breakable = breakable;
    }

    public bool IsBreakable()
    {
        return this.breakable;
    }
}
