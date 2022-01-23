using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    public Player player;


    // Update is called once per frame
    void Update()
    {
        if (player.GetHealt() <= 0)
        {
            player.GetEnemies().UpWinNum();
            player.KillAndErase();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            player.DownHealth();
        }
    }
}