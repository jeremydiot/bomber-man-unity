using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/* 
 * Class to manage game cells and its properties
 */
public class Cell
{
    // Player bonus
    public enum BonusType
    {
        None,
        MoreBomb,
        MoreDistance,
        InfiniteDistance
    }

    // Properties
    public bool isErasable = true;
    public BonusType bonusType = BonusType.None;
    
    private GameObject instanciateGameObject = null;

    private int posX; // Position X on game grid
    private int posY; // Position Y on game grid
    
    private bool isDrawable = true; // Forbidden draw gameObject positions

    public Cell(int posX, int posY, int[][] forbiddenDrawPositions = null)
    {
        this.posY = posY;
        this.posX = posX;

        if (forbiddenDrawPositions != null)
        {
            foreach (int[] position in forbiddenDrawPositions)
            {   
                // Disable gameObject drawing if position is disabled 
                if (this.posX == position[0] && this.posY == position[1]) isDrawable = false;
            }
        }
    }

    /*
     * Instantiate gameObject
     * return instantiate gameObject 
     */
    public GameObject Draw(GameObject gameObject, bool force = false)
    {
        if (this.instanciateGameObject == null && (isDrawable || force))
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)posX, (float)posY), gameObject.transform.rotation);
            return this.instanciateGameObject;
        }
        return null;
    }

    /*
     * Destroy gameObject
     */
    public void Erase(float delay = 0f, bool force = false){

        if (( this.isErasable || force ) && this.instanciateGameObject != null )
        {
            MonoBehaviour.Destroy(this.instanciateGameObject, delay);
            this.instanciateGameObject = null;
        }
    }
    
    /*
     * Get current gameObject
     */
    public GameObject GetInstantiateGameObject(){
        return this.instanciateGameObject;
    }

    /*
     * Get X position
     */
    public int GetPosX()
    {
        return this.posX;
    }

    /*
     * Get Y position
     */
    public int GetPosY()
    {
        return this.posY;
    }
}
