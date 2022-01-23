using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
    private bool canShoot = true;
    private bool infiniteImpact = false;
    private int maxBomb = 1;
    private int maxImpact = 1;
    private int availableBomb = 0;

    public Player(int number, string[] keyboard)
    {
        this.number = number;
        this.keyboard = keyboard;
    }

    public void startInfiniteImpact(int delay = 5000)
    {
        infiniteImpact = true;
        Task.Delay(delay).ContinueWith(t => { this.infiniteImpact = false; });
    }

    public bool isInfiniteImpact()
    {
        return this.infiniteImpact;
    }


    public int GetAvailableBomb()
    {
        return this.availableBomb;
    }

    public void SetAvailableBomb(int availableBomb)
    {

        this.availableBomb = availableBomb;
    }

    public int GetMaxBomb()
    {
        return this.maxBomb;
    }

    public void SetMaxBomb(int maxBomb)
    {
        this.maxBomb = maxBomb;
    }

    public void UpMaxBomb(int i = 1)
    {
        if (this.maxBomb == -1) return;

        this.maxBomb = this.maxBomb +i;
    }

    public int GetMaxImpact()
    {
        return this.maxImpact;
    }

    public void SetMaxImpact(int maxImpact)
    {
        this.maxImpact = maxImpact;
    }

    public void UpMaxImpact(int i = 1)
    {
        this.maxImpact = this.maxImpact + i;
    }

    public bool GetCanMove()
    {
        return this.canMove;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public bool GetCanShoot()
    {
        return this.canShoot;
    }

    public void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
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
            this.instanciateGameObject.GetComponent<Ability>().player = this;
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
