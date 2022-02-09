using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    public GameObject fireGameObject;
    public Player player;
    private float timerDestroy = 2.5f;
    
    void Start()
    {
        Destroy(gameObject, timerDestroy);
    }

    private void OnDestroy()
    {
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);
        
        if (player.maxDistance <= 0) return;

        Instantiate(fireGameObject, new Vector3(posX, posY), transform.rotation);

        bool[] directions = new bool[]
        {
            true, // left
            true, // right
            true, // top
            true // bottom
        };

        

        int maxY = GameManager.YLength -1;
        int maxX = GameManager.XLength -1;

        // instantiate fire on each directions  
        for (int i = 1; i <= player.maxDistance || player.IsInfiniteDistance(); i++)
        {

            if (!directions[0] && !directions[1] && !directions[2] && !directions[3]) break;
            Cell[] cells = new Cell[]
            {
                null, // left
                null, // right
                null, // top
                null // bottom
            };

            if (posX - i >= 0) // left
            {
                cells[0] = GameManager.Instance.mapCellsLayer[posX - i][posY];
            }

            if (posX + i <= maxX) // right
            {
                cells[1] = GameManager.Instance.mapCellsLayer[posX + i][posY];
            }

            if (posY + i <= maxY) // top
            {
                cells[2] = GameManager.Instance.mapCellsLayer[posX][posY + i];
            }

            if (posY - i >= 0) // bottom
            {
                cells[3] = GameManager.Instance.mapCellsLayer[posX][posY - i];
            }

            for (int j = 0; j < 4; j++)
            {
                if(cells[j] != null)
                {
                    if (cells[j].isErasable && directions[j])
                    {
                        Instantiate(fireGameObject, new Vector3(cells[j].GetPosX(), cells[j].GetPosY()), gameObject.transform.rotation);
                        if (cells[j].GetInstantiateGameObject() != null)
                        {
                            if(cells[j].GetInstantiateGameObject().tag == "BreakableWall")
                            {
                                directions[j] = false;
                            }
                        }
                    }
                    else
                    {
                        directions[j] = false;
                    }
                }
            }
        }
    }
}
