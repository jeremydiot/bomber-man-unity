using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private float timerDestroy = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);
    }

    private void OnDestroy()
    {
        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        Cell cell = GameManager.Instance.mapCellsLayer[(int)posY][(int)posX];
        cell.SetBonusType(Cell.BonusType.none);
    }
}
