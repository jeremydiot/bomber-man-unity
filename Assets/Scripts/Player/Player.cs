using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    private string[] keyboard;
    private int number;
    private int health = 10;
    private GameObject instanciateGameObject = null;
    private int winNum = 0;
    private Player enemy = null;
    private bool dead = false;
    private bool canMove = true;

    public Player(int number, string[] keyboard)
    {
        this.number = number;
        this.keyboard = keyboard;
    }

    public bool GetCanMove()
    {
        return this.canMove;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public bool IsDead()
    {
        return this.dead;
    }

    public Player GetEnemies()
    {
        return this.enemy;
    }

    public void setEnemy(Player player)
    {
        this.enemy = player;
    }

    public GameObject GetGameObject()
    {
        return this.instanciateGameObject;
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
        if (this.health < 0) this.health = 0;
        return this.health;
    }

    public int UpHealth(int i = 1){
        this.health = this.health + i;
        return this.health;
    }

    public void SetHealth(int i){
        this.health = i;
    }

    public int UpWinNum(int i = 1)
    {
        this.winNum = this.winNum + i;
        return this.winNum;
    }

    public int GetWinNum()
    {
        return this.winNum;
    }

    private void Draw(GameObject gameObject, int posX, int posY){
        if (this.instanciateGameObject == null){
            this.instanciateGameObject = MonoBehaviour.Instantiate(gameObject, new Vector3((float)posX, (float)posY), gameObject.transform.rotation);
            this.instanciateGameObject.GetComponent<Movement>().player = this;
            this.instanciateGameObject.GetComponent<Shoot>().player = this;
            this.instanciateGameObject.GetComponent<Health>().player = this;
        }
    }

    private void Erase(float delay = 0f){
        if (this.instanciateGameObject != null) MonoBehaviour.Destroy(this.instanciateGameObject, delay);
        this.instanciateGameObject = null;
        
    }

    public void KillAndErase(float delay = 0f){
        this.dead = true;
        this.Erase(delay);
    }

    public void RebornAndDraw(GameObject gameObject, int posX, int posY)
    {
        this.dead = false;
        this.Draw(gameObject, posX, posY);
    }
}
