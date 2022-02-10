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
        // end round when one player dead
        if (player.health <= 0 && !once)
        {
            // change the color when the player is dead
            player.ChangePlayerColor(Color.yellow);
            once = true;
            GameManager.Instance.EndRound(player);
        }

        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);
        
        // kill player when is lock between two unbreakable walls
        if (GameManager.Instance.mapCellsLayer[posX][posY].GetInstantiateGameObject() != null)
        {
            if (GameManager.Instance.mapCellsLayer[posX][posY].GetInstantiateGameObject().CompareTag("UnbreakableWall"))
            {
                player.health = 0;    
            }
        }
    }

    /**
     * down player health when is burned
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            player.health--;
        }
    }
}
