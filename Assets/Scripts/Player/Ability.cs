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
            Cell.BonusType bonusType = cell.UseBonus();

            if (bonusType == Cell.BonusType.moreBomb)
            {
                player.UpMaxBomb();
            }else if (bonusType == Cell.BonusType.moreImpact)
            {
                player.UpMaxImpact();
            }
            else if (bonusType == Cell.BonusType.infiniteImpact)
            {
                player.startInfiniteImpact();
            }
   
            Destroy(collision.gameObject);
        }
    }
}
