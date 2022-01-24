using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{

    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bonus"))
        {
            float bonusPosX = collision.gameObject.transform.position.x;
            float bonusPosY = collision.gameObject.transform.position.y;

            Cell cell = GameManager.Instance.mapCellsLayer[(int)bonusPosY][(int)bonusPosX];
            Cell.BonusType bonusType = cell.bonusType;

            if (bonusType == Cell.BonusType.moreBomb)
            {
                player.maxBomb++;
            }else if (bonusType == Cell.BonusType.moreDistance)
            {
                player.maxDistance++;
            }
            else if (bonusType == Cell.BonusType.infiniteDistance)
            {
                player.startInfiniteDistance();
            }
   
            Destroy(collision.gameObject);
        }
    }
}
