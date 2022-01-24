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

        bool right = true;
        bool left = true;
        bool top = true;
        bool bottom = true;

        for (int i = 1; i <= player.maxDistance || player.isInfiniteDistance(); i++)
        {

            if (!right && !left && !top && !bottom) break;

            try
            {
                if (GameManager.Instance.mapCellsLayer[(int)gameObject.transform.position.y][(int)gameObject.transform.position.x - i].erasable && left)
                    Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x - i, gameObject.transform.position.y), gameObject.transform.rotation);
                else
                    left = false;
            }
            catch (IndexOutOfRangeException) {
                left = false;
            }

            try
            {
                if (GameManager.Instance.mapCellsLayer[(int)gameObject.transform.position.y][(int)gameObject.transform.position.x + i].erasable && right)
                    Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x + i, gameObject.transform.position.y), gameObject.transform.rotation);
                else
                    right = false;
            }
            catch (IndexOutOfRangeException) {
                right = false;
            }

            try
            {
                if (GameManager.Instance.mapCellsLayer[(int)gameObject.transform.position.y - i][(int)gameObject.transform.position.x].erasable && bottom)
                    Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - i), gameObject.transform.rotation);
                else
                    bottom = false;
            }
            catch (IndexOutOfRangeException) {
                bottom = false;
            }

            try
            {
                if (GameManager.Instance.mapCellsLayer[(int)gameObject.transform.position.y + i][(int)gameObject.transform.position.x].erasable && top)
                    Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + i), gameObject.transform.rotation);
                else
                    top = false;
            }
            catch (IndexOutOfRangeException) {
                top = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
