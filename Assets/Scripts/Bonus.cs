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
     
        Color spriteColor;

        if (cell.GetInstantiateGameObject().tag == "Bonus")
        {
            if (bonusType == Cell.BonusType.MoreBomb)
            {
                spriteColor = Color.black;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
            else if (bonusType == Cell.BonusType.MoreDistance)
            {
                spriteColor = Color.red;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
            else if (bonusType == Cell.BonusType.InfiniteDistance)
            {
                spriteColor = Color.yellow;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
        }
    }

    private void OnDestroy()
    {
        Cell cell = GameManager.Instance.mapCellsLayer[(int)transform.position.x][(int)transform.position.y];
        cell.bonusType = Cell.BonusType.None;
    }
}
