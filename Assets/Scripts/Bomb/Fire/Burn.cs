using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{

    private float timerDestroy = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);

        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        GameManager.Instance.mapCellsLayer[posY][posX].Erase(1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
