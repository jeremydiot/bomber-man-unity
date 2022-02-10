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

        // count available bombs
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
        
        
        if (Input.GetKeyDown(keyboard[4]) && timers.Count < player.maxBomb)
        {
            int playerPosX = Mathf.RoundToInt(transform.position.x);
            int playerPoxY = Mathf.RoundToInt(transform.position.y);
            if (currentBomb == null) // if there is not bomb
            {
                timers.Add(0f);
                currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), bombGameObject.transform.rotation);
                currentBomb.GetComponent<Bomb>().player = player;
            }
            else
            {
                int currentBombPosX = Mathf.RoundToInt(currentBomb.transform.position.x);
                int currentBombPosY = Mathf.RoundToInt(currentBomb.transform.position.y);
                if(playerPosX != currentBombPosX || playerPoxY != currentBombPosY) // if last bomb is not at the same position
                {
                    timers.Add(0f);
                    currentBomb = Instantiate(bombGameObject, new Vector3(playerPosX, playerPoxY), bombGameObject.transform.rotation);
                    currentBomb.GetComponent<Bomb>().player = player;
                }
            }
        }
    }
}
