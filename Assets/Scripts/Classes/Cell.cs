using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Cell
{
    public enum BonusType
    {
        none,
        moreBomb,
        moreDistance,
        infiniteDistance
    }

    public bool erasable = true;
    public BonusType bonusType = BonusType.none;

    private GameObject instanciateGameObject = null;

    private int col = -1;
    private int row = -1;
    
    private bool canDraw = true;

    public Cell(int col, int row, int[][] forbiddenDrawPositions = null)
    {
        this.row = row;
        this.col = col;

        if (forbiddenDrawPositions != null)
        {
            foreach (int[] position in forbiddenDrawPositions)
            {
                if (this.col == position[0] && this.row == position[1]) canDraw = false;
            }
        }
    }

    public GameObject Draw(GameObject gameObject, bool force = false)
    {
        if (this.instanciateGameObject == null && (canDraw || force))
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)col, (float)row), gameObject.transform.rotation);
            return this.instanciateGameObject;
        }
        return null;
    }

    public void Erase(float delay = 0f, bool force = false){

        if (( this.erasable || force ) && this.instanciateGameObject != null )
        {
            MonoBehaviour.Destroy(this.instanciateGameObject, delay);
            this.instanciateGameObject = null;
        }
    }
    
    public GameObject GetInstanciateGameObject(){
        return this.instanciateGameObject;
    }

    public int getColNum()
    {
        return this.col;
    }

    public int getRowNum()
    {
        return this.row;
    }
}
