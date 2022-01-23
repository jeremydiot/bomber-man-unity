using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private float timerDestroy = 1f;
    public GameObject bonusPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);

        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        bool spawnBonus = (GameManager.Instance.mapCellsLayer[posY][posX].GetInstanciateGameObject() != null) && (GameManager.Instance.mapCellsLayer[posY][posX].GetBonusType() == Cell.BonusType.none);
        GameManager.Instance.mapCellsLayer[posY][posX].Erase(1f);
        if ((GameManager.Instance.mapCellsLayer[posY][posX].GetInstanciateGameObject() == null) && spawnBonus)
        {
            // TODO add random 1 chance on 3
            GameManager.Instance.mapCellsLayer[posY][posX].Draw(bonusPrefab);
            GameManager.Instance.mapCellsLayer[posY][posX].selectRandomBonus();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
