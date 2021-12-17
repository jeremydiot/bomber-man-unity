using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private static string[] forbiddenPositions = new string[12]{
        "1,11","2,11","1,10","15,1","14,1","15,2","15,11","14,11","15,10","1,1","1,2","2,1"
    };

    private bool isLock = false;
    private GameObject gameObject;
    private int col;
    private int line;

    public Cell(int col ,int line,bool isLock=false, GameObject gameObject=null)
    {
        foreach (string pos in forbiddenPositions)
        {
            if (col + "," + line == pos)
            {
                this.isLock = true;
                break;
            }
        }

        this.col = col;
        this.line = line;
        this.isLock = isLock;
        this.gameObject = gameObject;
    }

    public void setIsLock(bool isLock)
    {
        this.isLock = isLock;
    }
    
    public void setGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public bool getIsLock()
    {
        return this.isLock;
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    public void instanciateGameObject()
    {
        if (!this.isLock)
        {
            Instantiate(this.gameObject, new Vector3((float) col, (float) line), Quaternion.identity);
        }
    }

    public void destroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
