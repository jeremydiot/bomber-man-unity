using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private static int playerNumber = 2;
    private static int colNum = 17;
    private static int rowNum = 13;

    public static int[][] SpawnPositions = new int[][]
    {
        new int[] { 1, 1 },
        new int[] { 1, rowNum - 2},
        new int[] { colNum - 2, 1 },
        new int[] { colNum - 2, rowNum - 2 },
            
    };
    public static int[][] ForbiddenDrawPositions = new int[][]
    {
        new int[] { 1, 2 },
        new int[] { 1, 1 },
        new int[] { 2, 1 },

        new int[] { 1, rowNum - 3 },
        new int[] { 1, rowNum - 2 },
        new int[] { 2, rowNum - 2 },

        new int[] { colNum - 3, rowNum - 2 },
        new int[] { colNum - 2, rowNum - 2 },
        new int[] { colNum - 2, rowNum - 3 },

        new int[] { colNum - 2, 2},
        new int[] { colNum - 2, 1 },
        new int[] { colNum - 3, 1 },
    };
    public static string[][] playerKeyboard = new string[][]
    {
        new string[]{"z", "q", "s", "d", "space"},
        new string[]{"up", "left", "down", "right", "return"}
    };



    public GameObject groundPrefab;
    public GameObject UnbreakableWallPrefab;
    public GameObject BreakableWallPrefab;
    public GameObject PlayerPrefab;

    private Cell[][] groundCellsLayer = new Cell[rowNum][];
    public Cell[][] mapCellsLayer = new Cell[rowNum][];

    private Player[] players = new Player[playerNumber];

    private void Awake(){
        Instance = this;

        //init cells layers
        for (int r = 0; r < rowNum; r++)
        {
            groundCellsLayer[r] = new Cell[colNum];
            mapCellsLayer[r] = new Cell[colNum];

            for (int c = 0; c < colNum; c++)
            {
                groundCellsLayer[r][c]=new Cell(c, r, canErase:false);
                mapCellsLayer[r][c]=new Cell(c, r, ForbiddenDrawPositions);
            }
        }
    }

    // Start is called before the first frame update
    void Start(){
        DrawGroundCells();
        DrawUnbreakableGameCells();
        DrawBreakableGameCells();
        SpawnPlayer(playerNumber);
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void DrawGroundCells(){
        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {
                Cell cell = groundCellsLayer[r][c];
                cell.Draw(groundPrefab);
            }
        }
    }

    private void DrawUnbreakableGameCells()
    {
        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {

                Cell cell = mapCellsLayer[r][c];

                if (r == 0 || c == 0 || r == rowNum-1 || c == colNum-1 || (r % 2 == 0 && c % 2 == 0))
                {
                    cell.Draw(UnbreakableWallPrefab);
                    cell.SetErasable(false);
                }
            }
        }
    }
    
    private void DrawBreakableGameCells()
    {

        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {
                Cell cell = mapCellsLayer[r][c];

                if (cell.GetInstanciateGameObject() == null)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        cell.Draw(BreakableWallPrefab);
                    }
                }

            }
        }
    }
    private void SpawnPlayer(int num = 1)
    {

        int positionPlayerOne = Random.Range(0, 4);
        Player playerOne = new Player(1, playerKeyboard[0], 10);

        int positionPlayerTwo = Random.Range(2, 4);
        Player playerTwo = new Player(2, playerKeyboard[1], 10);

        if (num == 2)
        {
            positionPlayerOne = Random.Range(0, 2);

            players[1] = playerTwo;
            playerTwo.Draw(PlayerPrefab, SpawnPositions[positionPlayerTwo][0], SpawnPositions[positionPlayerTwo][1]);
        }

        players[0] = playerOne;
        playerOne.Draw(PlayerPrefab, SpawnPositions[positionPlayerOne][0], SpawnPositions[positionPlayerOne][1]);

    }
}

