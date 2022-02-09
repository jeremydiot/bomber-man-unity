using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;

        Cell cell = GameManager.Instance.mapCellsLayer[posX][posY];
        Cell.BonusType bonusType = cell.bonusType;

        SpriteRenderer spriteRenderer;

        if (cell.GetInstantiateGameObject().tag == "Bonus")
        {
            if (bonusType == Cell.BonusType.MoreBomb)
            {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.black;
            }
            else if (bonusType == Cell.BonusType.MoreDistance)
            {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.red;
            }
            else if (bonusType == Cell.BonusType.InfiniteDistance)
            {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.yellow;
            }
        }
    }

    private void OnDestroy()
    {
        Cell cell = GameManager.Instance.mapCellsLayer[(int)transform.position.x][(int)transform.position.y];
        cell.bonusType = Cell.BonusType.None;
    }
}
