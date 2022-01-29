using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    public Player player;
    public bool once = false;
    private float inWallTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        int playerPosX = (int)Mathf.Round(transform.position.x);
        int playerPoxY = (int)Mathf.Round(transform.position.y);
        
        if (player.health <= 0 && !once)
        {
            once = true;
            GameManager.Instance.endRound(player);
        }

        if (GameManager.Instance.mapCellsLayer[playerPoxY][playerPosX].GetInstanciateGameObject() != null)
        {
            if (GameManager.Instance.mapCellsLayer[playerPoxY][playerPosX].GetInstanciateGameObject().CompareTag("UnbreakableWall") && !once)
            {
                inWallTimer += Time.deltaTime;
                if (inWallTimer >= 1f)
                {
                    once = true;
                    GameManager.Instance.endRound(player);    
                }
            }
            else
            {
                inWallTimer = 0f;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            player.health--;
        }
    }
}
