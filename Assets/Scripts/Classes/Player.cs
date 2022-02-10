using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;

/*
 * Class to manage players and their properties
 */
public class Player
{
    // Game config properties
    private string[] keyboard;
    private int number;
    
    // Game player state and bonus
    public int winNum = 0;
    
    public int health;
    
    public int maxBomb;
    public int availableBomb;
    public int maxDistance;
    
    // Gameplay properties
    private GameObject instantiateGameObject = null;
    private Player enemy = null;
    private int countInfiniteDistance = 0;
    
    public Player(int number, string[] keyboard)
    {
        this.number = number;
        this.keyboard = keyboard;
    }

    /*
     * Disable all Player scripts
     */
    public void Freeze()
    {
        if (instantiateGameObject != null)
        {
            this.instantiateGameObject.GetComponent<Movement>().enabled = false;
            this.instantiateGameObject.GetComponent<Shoot>().enabled = false;
            this.instantiateGameObject.GetComponent<Health>().enabled = false;
            this.instantiateGameObject.GetComponent<Ability>().enabled = false;
        }
    }

    /*
     * Enable all player scripts
     */
    public void Unfreeze()
    {
        if (instantiateGameObject != null)
        {
            this.instantiateGameObject.GetComponent<Movement>().enabled = true;
            this.instantiateGameObject.GetComponent<Shoot>().enabled = true;
            this.instantiateGameObject.GetComponent<Health>().enabled = true;
            this.instantiateGameObject.GetComponent<Ability>().enabled = true;    
        }
    }

    /*
     * Reset player
     */
    public void Reset(GameObject playerGameObjectPrefab, int posX, int posY)
    {
        
        if(instantiateGameObject != null) MonoBehaviour.Destroy(instantiateGameObject);
        
        this.maxBomb = 1;
        this.availableBomb = 1;
        this.maxDistance = 1;
        this.health = 1;
        this.countInfiniteDistance = 0;
        
        // instantiate and init player gameObject
        this.instantiateGameObject = MonoBehaviour.Instantiate(playerGameObjectPrefab, new Vector3(posX, posY), playerGameObjectPrefab.transform.rotation);
        this.instantiateGameObject.GetComponent<Movement>().player = this;
        this.instantiateGameObject.GetComponent<Shoot>().player = this;
        this.instantiateGameObject.GetComponent<Health>().player = this;
        this.instantiateGameObject.GetComponent<Ability>().player = this;
    }

    /*
     * Infinite distance explode bonus
     */
    public void StartInfiniteDistance(int delay = 10000)
    {
        this.countInfiniteDistance ++;
        Task.Delay(10000).ContinueWith(t =>
        {
            if(this.countInfiniteDistance > 0) this.countInfiniteDistance--;
        });
    }

    /*
     * Get if infinite distance bonus is already enabled
     */
    public bool IsInfiniteDistance()
    {
        if (countInfiniteDistance <= 0) return false;
        return true;
    }

    /*
     * Get player number
     */
    public int GetNumber()
    {
        return this.number;
    }

    /*
     * Get player enemy
     */
    public Player GetEnemy()
    {
        return this.enemy;
    }

    /*
     * Set player enemy
     */
    public void SetEnemy(Player player)
    {
        this.enemy = player;
    }

    /*
     * Get player keyboard
     */
    public string[] GetKeyboard(){
        return this.keyboard;
    }
    
    /*
     * Get player instantiate gameObject
     */
    public GameObject GetInstantiateGameObject()
    {
        return this.instantiateGameObject;
    }

  /**
   * Change player color
   */
  public void ChangePlayerColor(Color color)
  {
    if(this.instantiateGameObject != null)
    {
      this.instantiateGameObject.GetComponent<SpriteRenderer>().color = color;
    }
  }
}
