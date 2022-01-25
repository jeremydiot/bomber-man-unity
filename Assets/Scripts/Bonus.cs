using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        int posX = (int)this.gameObject.transform.position.x;
        int posY = (int)this.gameObject.transform.position.y;

        Cell cell = GameManager.Instance.mapCellsLayer[posY][posX];
        Cell.BonusType bonusType = cell.bonusType;

        SpriteRenderer spriteRenderer;
     
        Color spriteColor;

        if (cell.GetInstanciateGameObject().tag == "Bonus")
        {
            if (bonusType == Cell.BonusType.moreBomb)
            {
                spriteColor = Color.black;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
            else if (bonusType == Cell.BonusType.moreDistance)
            {
                spriteColor = Color.red;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
            else if (bonusType == Cell.BonusType.infiniteDistance)
            {
                spriteColor = Color.yellow;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
        }
    }

    private void OnDestroy()
    {
        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        Cell cell = GameManager.Instance.mapCellsLayer[(int)posY][(int)posX];
        cell.bonusType = Cell.BonusType.none;
    }
}
