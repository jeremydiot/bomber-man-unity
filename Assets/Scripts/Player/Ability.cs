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
            int bonusPosX = Mathf.RoundToInt(collision.gameObject.transform.position.x);
            int bonusPosY = Mathf.RoundToInt(collision.gameObject.transform.position.y);

            Cell cell = GameManager.Instance.mapCellsLayer[bonusPosX][bonusPosY];
            Cell.BonusType bonusType = cell.bonusType;

            if (bonusType == Cell.BonusType.MoreBomb)
            {
                player.maxBomb++;
            }else if (bonusType == Cell.BonusType.MoreDistance)
            {
                player.maxDistance++;
            }
            else if (bonusType == Cell.BonusType.InfiniteDistance)
            {
                player.StartInfiniteDistance();
            }
            Destroy(collision.gameObject);
        }
    }
}
