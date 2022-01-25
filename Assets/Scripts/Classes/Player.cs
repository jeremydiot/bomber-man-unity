using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;

public class Player
{
    // config
    private string[] keyboard;
    private int number;

    // state
    public int health = 1;
    public int winNum = 0;

    public int maxBomb = 1;
    public int availableBomb = 1;
    public int maxDistance = 1;

    public bool canMove = true;
    public bool canShoot = true;

    private bool dead = false;

    // other
    private GameObject instanciateGameObject = null;
    private Player enemy = null;
    private int countInfiniteDistance = 0;

    public Player(int number, string[] keyboard)
    {
        this.number = number;
        this.keyboard = keyboard;
    }

    public void reset()
    {
        this.maxBomb = 1;
        this.availableBomb = 1;
        this.maxDistance = 1;
        this.health = 1;
    }

    public void startInfiniteDistance(int delay = 10000)
    {
        this.countInfiniteDistance ++;
        Task.Delay(delay).ContinueWith(t =>
        {
            this.countInfiniteDistance--;
        });
    }

    public bool isInfiniteDistance()
    {
        if (countInfiniteDistance <= 0) return false;
        return true;
    }

    public bool IsDead()
    {
        return this.dead;
    }

    public void KillAndErase(float delay = 0f)
    {
        this.dead = true;
        this.Erase(delay);
    }

    public void ResuscitAndDraw(GameObject gameObject, int posX, int posY)
    {
        this.dead = false;
        this.Draw(gameObject, posX, posY);
    }

    private void Draw(GameObject gameObject, int posX, int posY)
    {
        if (this.instanciateGameObject == null)
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)posX, (float)posY), gameObject.transform.rotation);
            this.instanciateGameObject.GetComponent<Movement>().player = this;
            this.instanciateGameObject.GetComponent<Shoot>().player = this;
            this.instanciateGameObject.GetComponent<Health>().player = this;
            this.instanciateGameObject.GetComponent<Ability>().player = this;

        }

    }

    private void Erase(float delay = 0f)
    {
        if (this.instanciateGameObject != null) MonoBehaviour.Destroy(this.instanciateGameObject, delay);
        this.instanciateGameObject = null;

    }

    public int GetNumber()
    {
        return this.number;
    }

    public Player GetEnemy()
    {
        return this.enemy;
    }

    public void setEnemy(Player player)
    {
        this.enemy = player;
    }

    public string[] GetKeyboard(){
        return this.keyboard;
    }
}
