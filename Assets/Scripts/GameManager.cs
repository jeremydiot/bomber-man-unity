using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int[][] SpawnPositions = new int[][]
    {
            new int[] { 1, 1 },
            new int[] { 1, 11 },
            new int[] { 15, 11 },
            new int[] { 15, 1 },
            
    };

    public GameObject groundGameObject;
    public GameObject UnbreakableGameObject;
    public GameObject BreakableGameObject;
    public GameObject PlayerGameObject;

    private List<List<Cell>> groundCells = new List<List<Cell>>();
    private List<List<Cell>> mapCells = new List<List<Cell>>();
    private List<GameObject> instantiatePlayerGameObjects = new List<GameObject>();


    private int colNum = 16;
    private int rowNum = 12;

    private void Awake(){
        Instance = this;

        //empty ground and map cells
        for (int r = 0; r <= rowNum; r++)
        {
            groundCells.Add(new List<Cell>());
            mapCells.Add(new List<Cell>());

            for (int c = 0; c <= colNum; c++)
            {
                groundCells[r].Add(new Cell(c,r));
                mapCells[r].Add(new Cell(c,r));
            }
        }

    }
    // Start is called before the first frame update
    void Start(){
        DrawGroundCells();
        DrawUnbreakableGameCells();
        DrawBreakableGameCells();
        SpawnPlayer(2);
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void DrawGroundCells(){
        for (int r = 0; r <= rowNum; r++)
        {
            for (int c = 0; c <= colNum; c++)
            {
                Cell cell = groundCells[r][c];
                cell.SetGameObject(groundGameObject);
                cell.SetBreakable(false);
                cell.Draw(true);
            }
        }
    }

    private void DrawUnbreakableGameCells()
    {
        for (int r = 0; r <= rowNum; r++)
        {
            for (int c = 0; c <= colNum; c++)
            {

                Cell cell = mapCells[r][c];

                if (r == rowNum || r == 0 || c == colNum || c == 0)
                {
                    cell.SetGameObject(UnbreakableGameObject);
                    cell.SetBreakable(false);
                    cell.Draw();

                }
                else if (r % 2 == 0 && c % 2 == 0)
                {

                    cell.SetGameObject(UnbreakableGameObject);
                    cell.SetBreakable(false);
                    cell.Draw();
                }
            }
        }
    }

    private void DrawBreakableGameCells()
    {

        for (int r = 0; r <= rowNum; r++)
        {
            for (int c = 0; c <= colNum; c++)
            {

                Cell cell = mapCells[r][c];

                if (cell.GetGameObject() == null)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        cell.SetGameObject(BreakableGameObject);
                        cell.SetBreakable(true);
                        cell.Draw();
                    }
                }

            }
        }
    }

    private void SpawnPlayer(int num = 1){
        int positionPlayerOne = Random.Range(0, 4);
        int positionPlayerTwo = 1;

        if (positionPlayerOne == 0) positionPlayerTwo = 2;
        else if (positionPlayerOne == 1) positionPlayerTwo = 3;
        else if (positionPlayerOne == 2) positionPlayerTwo = 0;

        GameObject instantiateGameObjectPlayerOne =  Instantiate(PlayerGameObject, new Vector3((float)SpawnPositions[positionPlayerOne][0], (float)SpawnPositions[positionPlayerOne][1]), PlayerGameObject.transform.rotation);
        instantiateGameObjectPlayerOne.GetComponent<Movement>().playerNum=1;
        instantiateGameObjectPlayerOne.GetComponent<Shoot>().playerNum=1;
        instantiatePlayerGameObjects.Add(instantiateGameObjectPlayerOne);

        if (num > 1)
        {
            GameObject instantiateGameObjectPlayerTwo = Instantiate(PlayerGameObject, new Vector3((float)SpawnPositions[positionPlayerTwo][0], (float)SpawnPositions[positionPlayerTwo][1]), PlayerGameObject.transform.rotation);
            instantiateGameObjectPlayerTwo.GetComponent<Movement>().playerNum = 2;
            instantiateGameObjectPlayerTwo.GetComponent<Shoot>().playerNum = 2;
            instantiatePlayerGameObjects.Add(instantiateGameObjectPlayerTwo);

        }

    }
}

