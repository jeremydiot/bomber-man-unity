using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public Player player;
    public GameObject bombGameObject;

    private string[] keyboard; 
    private float timer = 2f;
    private float keyboardDelay = 2f;
    private int currentShootNum = 0;
    
    private GameObject currentBomb = null;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        keyboard = player.GetKeyboard();
    }

    // Update is called once per frame
    void Update()
    {

        player.SetAvailableBomb(player.GetMaxBomb()-currentShootNum);


        if(currentShootNum >= player.GetMaxBomb()) timer += Time.deltaTime;


        if (timer >= keyboardDelay)
        {
            timer = 0;
            currentShootNum = 0;
        }

        if (Input.GetKey(keyboard[4]))
        {

            if (currentShootNum < player.GetMaxBomb() && player.GetCanShoot())
            {
                int playerPosX = (int)Mathf.Round(transform.position.x);
                int playerPoxY = (int)Mathf.Round(transform.position.y);


                if (currentBomb == null)
                {
                    currentShootNum++;
                    currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), transform.rotation);
                    currentBomb.GetComponent<Bomb>().player = player;
                }
                else
                {
                    
                    int currentBombPosX = (int)currentBomb.transform.position.x;
                    int currentBombPosY = (int)currentBomb.transform.position.y;

                    if(playerPosX != currentBombPosX || playerPoxY != currentBombPosY)
                    {
                        currentShootNum++;
                        currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), transform.rotation);
                        currentBomb.GetComponent<Bomb>().player = player;
                    }
                }
            }
        }
    }
}
