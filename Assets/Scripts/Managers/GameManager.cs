using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // provide acces to general game information
    public static GameManager Instance;

    // static games parameters and configuration
    public static int maxRoundNum = 3;
    public static float maxTimeNum = 120f;

    public static int colNum = 17;
    public static int rowNum = 13;

    private static int breakableWallNum = 40;
    private static int infiniteDistanceBonusNum = 6;
    private static int moreBombBonus = 6;   
    private static int moreDistanceBonus = 8;

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

    // interface elements
    public GameObject groundPrefab;
    public GameObject UnbreakableWallPrefab;
    public GameObject BreakableWallPrefab;
    public GameObject PlayerPrefab;

    private GamePanels panels;

    // game dynamique variables
    public Cell[][] mapCellsLayer = new Cell[rowNum][];

    public Player[] players = new Player[]{
        new Player(1, new string[]{"z", "q", "s", "d", "space"}),
        new Player(2, new string[]{"up", "left", "down", "right", "return"})
    };

    public int currentRoundNum = 1;
    public float currentTime = maxTimeNum;
    public string finishMessage;

    private void Awake(){
        Instance = this;

        panels = gameObject.GetComponent<GamePanels>();
        
        //init cells layer
        for (int r = 0; r < rowNum; r++)
        {
            mapCellsLayer[r] = new Cell[colNum];

            for (int c = 0; c < colNum; c++)
            {
                mapCellsLayer[r][c]=new Cell(c, r, ForbiddenDrawPositions);
                Instantiate(groundPrefab, new Vector3(c,r), groundPrefab.transform.rotation);
            }
        }
    }
    
    void Start(){
        resetGame(0f);
    }

    private void drawMapCells()
    {
        int currentBreakableWallNum = breakableWallNum;
        int currentInfiniteDistanceBonusNum = infiniteDistanceBonusNum;
        int currentMoreBombBonus = moreBombBonus;
        int currentMoreDistanceBonus = moreDistanceBonus;

        List<Cell> possibleBonusCells = new List<Cell>();

        // unBreakableWall
        for (int r = 0; r < mapCellsLayer.Length; r++)
        {
            for (int c = 0; c < mapCellsLayer[r].Length; c++)
            {

                Cell cell = mapCellsLayer[r][c];

                if (r == 0 || c == 0 || r == rowNum - 1 || c == colNum - 1 || (r % 2 == 0 && c % 2 == 0))
                {
                    cell.erasable = false;
                    cell.Draw(UnbreakableWallPrefab);
                }
            }
        }

        // breakableWall
        while (currentBreakableWallNum > 0){
            int r = Random.Range(1, rowNum-1);
            int c = Random.Range(1, colNum-1);

            if (mapCellsLayer[r][c].Draw(BreakableWallPrefab) != null)
            {

                possibleBonusCells.Add(mapCellsLayer[r][c]);
                currentBreakableWallNum--;
            }
        }

        // add bonus
        foreach (Cell cell in possibleBonusCells)
        {
            if (currentInfiniteDistanceBonusNum > 0)
            {
                cell.bonusType = Cell.BonusType.infiniteDistance;
                currentInfiniteDistanceBonusNum--;
                continue;
                
            }
            else if (currentMoreBombBonus > 0)
            {
                cell.bonusType = Cell.BonusType.moreBomb;
                currentMoreBombBonus--;
                continue;
            }
            else if (currentMoreDistanceBonus > 0)
            {
                cell.bonusType = Cell.BonusType.moreDistance;
                currentMoreDistanceBonus--;
                continue;
            }
            
            break;

        }
    }

    public void endRound(Player deadPlayer)
    {
        lockGame();

        Player winPlayer = deadPlayer.GetEnemy();
        winPlayer.winNum++;
        if (deadPlayer.winNum > winPlayer.winNum) winPlayer = deadPlayer;
        
        bool equality = deadPlayer.winNum == deadPlayer.GetEnemy().winNum;

        if ((currentRoundNum >= maxRoundNum || winPlayer.winNum > maxRoundNum/2) && maxRoundNum > 0)
        {
            currentTime = maxTimeNum;
            lockGame();
            
            if (equality) finishMessage = "EQUALITY !";
            else finishMessage = "PLAYER " + winPlayer.GetNumber().ToString() + " WIN !";
            
            Invoke("endGameProcess",1f);

        }
        else
        {
            currentRoundNum++;
            resetGame();
        }
    }

    private void endGameProcess()
    {
        cleanGame();
        panels.Finish(finishMessage);
    }

    public void lockGame()
    {
        gameObject.GetComponent<TimeOver>().enabled = false;

        players[0].freeze();
        players[1].freeze();
    }

    public void unlockGame()
    {
        gameObject.GetComponent<TimeOver>().enabled = true;
        
        players[0].unfreeze();
        players[1].unfreeze();
    }

    private void resetGame(float delay = 1f)
    {
        currentTime = maxTimeNum;
        lockGame();
        Invoke("resetProcessStart",delay);
    }
    private void resetProcessStart()
    {
        panels.Message("Round " + currentRoundNum.ToString());
        Invoke("resetProcessEnd",2f);
        cleanGame();
        drawMapCells();
        spawnPlayer();
        lockGame();
    }
    
    private void resetProcessEnd()
    {
        panels.Message("", false);
        unlockGame();
    }
    
    private void spawnPlayer(){

        int positionPlayerOne = Random.Range(0, 2);
        int positionPlayerTwo = Random.Range(2, 4);

        players[0].reset();
        players[1].reset();

        players[0].Draw(PlayerPrefab, SpawnPositions[positionPlayerOne][0], SpawnPositions[positionPlayerOne][1]);
        players[1].Draw(PlayerPrefab, SpawnPositions[positionPlayerTwo][0], SpawnPositions[positionPlayerTwo][1]);
    
        players[0].setEnemy(players[1]);
        players[1].setEnemy(players[0]);

    }
    
    private void cleanGame(){

        foreach (Player player in players)player.Erase();

        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bomb")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bonus")) Destroy(fire);

        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {
                mapCellsLayer[r][c].Erase(force:true);
                mapCellsLayer[r][c].erasable = true;
                mapCellsLayer[r][c].bonusType = Cell.BonusType.none;
            }
        }
    }
}

