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
        moreImpact,
        infiniteImpact
    }


    private GameObject instanciateGameObject = null;
    private int col = -1;
    private int row = -1;
    private bool canErase = true;
    private bool canDraw = true;
    private BonusType bonusType = BonusType.none;

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

    public BonusType GetBonusType()
    {
        return bonusType;
    }

    public void SetBonusType(BonusType bonusType = BonusType.none)
    {
        this.bonusType = bonusType;
    }

    public BonusType UseBonus()
    {
        BonusType currentBonusType = this.GetBonusType(); 
        this.SetBonusType(BonusType.none);
        return currentBonusType;
    }

    public BonusType selectRandomBonus()
    {
        int random = Random.Range(0, 10);

        if (random < 2)
        {
            this.bonusType = BonusType.infiniteImpact;
        }
        else if (random < 5)
        {
            this.bonusType= BonusType.moreBomb;
        }
        else
        {
            this.bonusType = BonusType.moreImpact;
        }
        return this.bonusType;
    }


    public GameObject Draw(GameObject gameObject, bool force = false)
    {
        if (this.instanciateGameObject == null && (force || canDraw))
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)col, (float)row), gameObject.transform.rotation);
            return this.instanciateGameObject;
        }
        return null;
    }

    public void Erase(float delay = 0f, bool force = false){

        if (this.IsErasable()||force){
            MonoBehaviour.Destroy(this.instanciateGameObject, delay);
            this.instanciateGameObject = null;
            this.SetErasable(true);
            this.bonusType = BonusType.none;
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
