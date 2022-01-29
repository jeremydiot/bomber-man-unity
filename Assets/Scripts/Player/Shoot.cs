using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public Player player;
    public GameObject bombGameObject;

    private string[] keyboard;
    private List<float> timers = new List<float>();
    private float maxTimer = 2.5f;
    private GameObject currentBomb = null;
    
    void Start()
    {
        keyboard = player.GetKeyboard();
    }
    
    void Update()
    {
        player.availableBomb = player.maxBomb - timers.Count;

        for (int i = 0; i < timers.Count; i++)
        {
            timers[i] += Time.deltaTime;
        }

        for (int i = 0; i < timers.Count; i++)
        {
            if(timers[i] >= maxTimer)
            {
                timers.RemoveAt(i);
            }
        }
        
        if (Input.GetKey(keyboard[4]) && timers.Count < player.maxBomb)
        {
            int playerPosX = (int)Mathf.Round(transform.position.x);
            int playerPoxY = (int)Mathf.Round(transform.position.y);
            
            if (currentBomb == null)
            {
                timers.Add(0f);
                currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), transform.rotation);
                currentBomb.GetComponent<Bomb>().player = player;
            }
            else
            {
                int currentBombPosX = (int)currentBomb.transform.position.x;
                int currentBombPosY = (int)currentBomb.transform.position.y;

                if(playerPosX != currentBombPosX || playerPoxY != currentBombPosY)
                {
                    timers.Add(0f);
                    currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), transform.rotation);
                    currentBomb.GetComponent<Bomb>().player = player;
                }
            }
        }
    }
}
