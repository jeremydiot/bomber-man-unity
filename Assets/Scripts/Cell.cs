using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Cell
{

    private GameObject instanciateGameObject = null;
    private int col = -1;
    private int row = -1;
    private bool canErase = true;
    private bool canDraw = true;

    public Cell(int col, int row, int[][] forbiddenDrawPositions = null, bool canErase = true )
    {
        this.row = row;
        this.col = col;
        this.canErase = canErase;

        if (forbiddenDrawPositions != null)
        {
            foreach (int[] position in forbiddenDrawPositions)
            {
                if (this.col == position[0] && this.row == position[1]) canDraw = false;
            }
        }
    }


    public void Draw(GameObject gameObject, bool force = false)
    {
        if (this.instanciateGameObject == null && (force || canDraw))
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)col, (float)row), gameObject.transform.rotation);
        }
    }

    public void Erase(float delay = 0f, bool force = false){

        if (this.IsErasable()||force){
            MonoBehaviour.Destroy(this.instanciateGameObject, delay);
            this.instanciateGameObject = null;
            this.SetErasable(true);
        }
    }
    public void SetErasable(bool canErase)
    {
        this.canErase = canErase;
    }

    public bool IsErasable()
    {
        return this.canErase;
    }
    
    public GameObject GetInstanciateGameObject(){
        return this.instanciateGameObject;
    }

}
