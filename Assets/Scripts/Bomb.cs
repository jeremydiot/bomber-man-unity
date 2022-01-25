using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    public GameObject fireGameObject;
    public Player player;
    private float timerDestroy = 2.6f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);
        Invoke("instantiateFire", 2.5f);
    }

    public void instantiateFire()
    {

        if (player.maxDistance <= 0) return;

        Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), gameObject.transform.rotation);

        bool[] directions = new bool[]
        {
            true, // left
            true, // right
            true, // top
            true // bottom
        };

        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        int maxY = GameManager.rowNum -1;
        int maxX = GameManager.colNum -1;

        for (int i = 1; i <= player.maxDistance || player.isInfiniteDistance(); i++)
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
                cells[0] = GameManager.Instance.mapCellsLayer[posY][posX - i];
            }

            if (posX + i <= maxX) // right
            {
                cells[1] = GameManager.Instance.mapCellsLayer[posY][posX + i];
            }

            if (posY + i <= maxY) // top
            {
                cells[2] = GameManager.Instance.mapCellsLayer[posY + i][posX];
            }

            if (posY - i >= 0) // bottom
            {
                cells[3] = GameManager.Instance.mapCellsLayer[posY - i][posX];
            }

            for (int j = 0; j < 4; j++)
            {
                if(cells[j] != null)
                {
                    if (cells[j].erasable && directions[j])
                    {
                        Instantiate(fireGameObject, new Vector3(cells[j].getColNum(), cells[j].getRowNum()), gameObject.transform.rotation);
                        if (cells[j].GetInstanciateGameObject() != null)
                        {
                            if(cells[j].GetInstanciateGameObject().tag == "BreakableWall")
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
    

    // Update is called once per frame
    void Update()
    {

    }
}
