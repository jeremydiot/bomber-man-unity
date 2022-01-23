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

        int posX = (int)this.gameObject.transform.position.x;
        int posY = (int)this.gameObject.transform.position.y;
        Cell cell = GameManager.Instance.mapCellsLayer[posY][posX];
        Cell.BonusType bonusType = cell.GetBonusType();

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
            else if (bonusType == Cell.BonusType.moreImpact)
            {
                spriteColor = Color.red;
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = spriteColor;
            }
            else if (bonusType == Cell.BonusType.infiniteImpact)
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
        cell.SetBonusType(Cell.BonusType.none);
    }
}
