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

        


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        int posX = (int)gameObject.transform.position.x;
        int posY = (int)gameObject.transform.position.y;

        if(GameManager.Instance.mapCellsLayer[posY][posX].GetInstanciateGameObject() != null)
        {
            if (GameManager.Instance.mapCellsLayer[posY][posX].GetInstanciateGameObject().tag != "Bonus")
            {
                GameManager.Instance.mapCellsLayer[posY][posX].Erase();

                if ((GameManager.Instance.mapCellsLayer[posY][posX].GetInstanciateGameObject() == null) && GameManager.Instance.mapCellsLayer[posY][posX].bonusType != Cell.BonusType.none)
                {
                    GameManager.Instance.mapCellsLayer[posY][posX].Draw(bonusPrefab);
                }
            }
        }
    }
}
