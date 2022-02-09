using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private float timerDestroy = 1f;
    public GameObject bonusPrefab;

    void Start()
    {
        Destroy(gameObject, timerDestroy);
    }

    private void OnDestroy()
    {
        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        // if not bonus erase cell gameObject on destroy
        if(GameManager.Instance.mapCellsLayer[posX][posY].GetInstantiateGameObject() != null)
        {
            if (!GameManager.Instance.mapCellsLayer[posX][posY].GetInstantiateGameObject().CompareTag("Bonus"))
            {
                GameManager.Instance.mapCellsLayer[posX][posY].Erase();

                if ((GameManager.Instance.mapCellsLayer[posX][posY].GetInstantiateGameObject() == null) && GameManager.Instance.mapCellsLayer[posX][posY].bonusType != Cell.BonusType.None)
                {
                    GameManager.Instance.mapCellsLayer[posX][posY].Draw(bonusPrefab);
                }
            }
        }
    }
}
