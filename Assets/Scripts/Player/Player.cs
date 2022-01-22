using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public string[] keyboard;
    private int number;
    private int health;
    private GameObject instanciateGameObject = null;

    public Player(int number, string[] keyboard, int health){
        this.number = number;
        this.health = health;
        this.keyboard = keyboard;
    }

    public int GetNumber(){
        return this.number;
    }

    public string[] GetKeyboard(){
        return this.keyboard;
    }

    public int GetHealt(){
        return this.health;
    }

    public int DownHealth(int i = 1){
        this.health = this.health - i;
        return this.health;
    }

    public int UpHealth(int i = 1)
    {
        this.health = this.health + i;
        return this.health;
    }

    public void Draw(GameObject gameObject, int posX, int posY)
    {
        if (this.instanciateGameObject == null)
        {
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)posX, (float)posY), gameObject.transform.rotation);
            this.instanciateGameObject.GetComponent<Movement>().player = this;
            this.instanciateGameObject.GetComponent<Shoot>().player = this;
            this.instanciateGameObject.GetComponent<Health>().player = this;
        }
    }

    public void Erase(float delay = 0f)
    {
        MonoBehaviour.Destroy(this.instanciateGameObject, delay);
        this.instanciateGameObject = null;
    }
}
