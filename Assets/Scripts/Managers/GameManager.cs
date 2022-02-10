using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

/*
 * Class to centralize general gameplay variables and functionalities
 */
public class GameManager : MonoBehaviour
{
    // Provide access to current general game variables
    public static GameManager Instance;

    // Static games parameters and configuration
    public static int MaxRoundNum = 3; // Init by MenuManager
    public static float MaxTimeNum = 120f; // Init by MenuManager

    public static readonly int ColNum = 17; // X game board length to 0 to 16 
    public static readonly int RowNum = 13; // Y game board length to 0 to 12
    
    public static readonly int XLength = ColNum;
    public static readonly int YLength = RowNum;
    
    // Max number for each in game
    private static readonly int BreakableWallNum = 40;
    private static readonly int InfiniteDistanceBonusNum = 6;
    private static readonly int MoreBombBonus = 6;   
    private static readonly int MoreDistanceBonus = 8;

    // Player Spawn Positions 
    private static readonly int[][] SpawnPositions = new int[][]
    {
        new int[] { 1, 1 },
        new int[] { 1, YLength - 2 },
        new int[] { XLength - 2, 1 },
        new int[] { XLength - 2, YLength - 2 },
            
    };

    // Unauthorized gameObject instantiate position  
    private static readonly int[][] ForbiddenDrawPositions = new int[][]
    {
        new int[] { 1, 2 },
        new int[] { 1, 1 },
        new int[] { 2, 1 },

        new int[] { 1, YLength - 3 },
        new int[] { 1, YLength - 2 },
        new int[] { 2, YLength - 2 },

        new int[] { XLength - 3, YLength - 2 },
        new int[] { XLength - 2, YLength - 2 },
        new int[] { XLength - 2, YLength - 3 },

        new int[] { XLength - 2, 2 },
        new int[] { XLength - 2, 1 },
        new int[] { XLength - 3, 1 },
    };

    // Interface prefab elements
    public GameObject groundPrefab;
    public GameObject unbreakableWallPrefab;
    public GameObject breakableWallPrefab;
    public GameObject playerPrefab;

    // Game planels
    private GamePanels panels;

    // Game variables
    public readonly Cell[][] mapCellsLayer = new Cell[XLength][];
    public Player[] players;

    public int currentRoundNum = 1;
    public float currentTime = MaxTimeNum;
    private string finishMessage;

    // Function to browse all game cell
    private void ForEachCells(Func<int,int,Cell,Cell> callback)
    {
        // Init cells layer
        for (int x = 0; x < mapCellsLayer.Length; x++)
        {
            for (int y = 0; y < mapCellsLayer[x].Length; y++)
            {
                Cell cell = callback(x,y,mapCellsLayer[x][y]);
                if (cell != null)
                {
                    mapCellsLayer[x][y] = cell;
                }
            }
        }
    }
    
    private void Awake(){
        Instance = this;
        panels = gameObject.GetComponent<GamePanels>();

        // Init cells layer
        for (int x = 0; x < XLength; x++){mapCellsLayer[x] = new Cell[YLength];}
        
        // Create ground game board and create cells
        ForEachCells((int x,int y, Cell cell) =>
        {
            Instantiate(groundPrefab, new Vector3(x,y), groundPrefab.transform.rotation);
            return new Cell(x, y, ForbiddenDrawPositions);
        });
        
        // Init players
        players = new Player[]{
            new Player(1, new string[]{"z", "q", "s", "d", "space"}),
            new Player(2, new string[]{"up", "left", "down", "right", "return"})
        };
        players[0].SetEnemy(players[1]);
        players[1].SetEnemy(players[0]);
    }
    
    void Start(){
        StartCoroutine(ResetGame(0));
    }

    /*
     * Function to instantiate board gameObjects
     */
    private void DrawMapCells()
    {
        int currentBreakableWallNum = BreakableWallNum;
        int currentInfiniteDistanceBonusNum = InfiniteDistanceBonusNum;
        int currentMoreBombBonus = MoreBombBonus;
        int currentMoreDistanceBonus = MoreDistanceBonus;

        List<Cell> possibleBonusCells = new List<Cell>();

        // UnBreakableWall
        ForEachCells((x, y, cell) =>
        {
            if (y == 0 || x == 0 || y == YLength - 1 || x == XLength - 1 || (y % 2 == 0 && x % 2 == 0))
            {
                cell.isErasable = false;
                cell.Draw(unbreakableWallPrefab);
            }
            return null;
        });

        // BreakableWall
        while (currentBreakableWallNum > 0){
            int y = Random.Range(1, YLength-1);
            int x = Random.Range(1, XLength-1);

            if (mapCellsLayer[x][y].Draw(breakableWallPrefab) != null)
            {
                possibleBonusCells.Add(mapCellsLayer[x][y]);
                currentBreakableWallNum--;
            }
        }

        // add bonus to BreakableWall
        foreach (Cell cell in possibleBonusCells)
        {
            if (currentInfiniteDistanceBonusNum > 0)
            {
                cell.bonusType = Cell.BonusType.InfiniteDistance;
                currentInfiniteDistanceBonusNum--;
                continue;
                
            }
            else if (currentMoreBombBonus > 0)
            {
                cell.bonusType = Cell.BonusType.MoreBomb;
                currentMoreBombBonus--;
                continue;
            }
            else if (currentMoreDistanceBonus > 0)
            {
                cell.bonusType = Cell.BonusType.MoreDistance;
                currentMoreDistanceBonus--;
                continue;
            }
            
            break;
        }
    }
    
    /*
     * Function to reset all cells and destroy all gameplay objects
     */
    private void CleanGame(){
        // Destroy all gameplay objects for security
        DestroyAllGameObjects();
        // Reset each cells
        ForEachCells((x, y, cell) =>
        {
            cell.isErasable = true;
            cell.Erase();
            cell.bonusType = Cell.BonusType.None;
            
            return null;
        });
    }

    /*
     * Function to lock players and timer
     */
    public void LockGame()
    {
        gameObject.GetComponent<TimeOver>().enabled = false;
        players[0].Freeze();
        players[1].Freeze();
    }

    /*
     * Function to unlock players and timer
     */
    public void UnlockGame()
    {
        gameObject.GetComponent<TimeOver>().enabled = true;
        players[0].Unfreeze();
        players[1].Unfreeze();
    }
    
    /*
     * Function called when one player is dead 
     */
    public void EndRound(Player deadPlayer)
    {
        LockGame();
        DestroyAllGameObjects();
        // end stats
        Player winPlayer = deadPlayer.GetEnemy();
        winPlayer.winNum++;
        if (deadPlayer.winNum > winPlayer.winNum) winPlayer = deadPlayer;
        
        bool equality = deadPlayer.winNum == deadPlayer.GetEnemy().winNum;

        // max round or player win => end game
        if ((currentRoundNum >= MaxRoundNum || (winPlayer.winNum > MaxRoundNum/2) && MaxRoundNum > 0))
        {
            currentTime = MaxTimeNum;

            if (equality) finishMessage = "EQUALITY !";
            else finishMessage = "PLAYER " + winPlayer.GetNumber().ToString() + " WIN !";
            
            StartCoroutine(EndGame());
        }
        // new round
        else
        {
            currentRoundNum++;
            StartCoroutine(ResetGame());
        }
    }

    /*
     * Display end game result
     */
    private IEnumerator EndGame()
    {
        
        LockGame();
        yield return new WaitForSeconds(1);
        
        currentTime = MaxTimeNum;
        
        CleanGame();
        panels.Finish(finishMessage);
    }

    /*
     * Function to init or reinit game
     */
    private IEnumerator ResetGame(int timer = 1)
    {
        LockGame();
        yield return new WaitForSeconds(timer);
        
        currentTime = MaxTimeNum;
        panels.Message("Round " + currentRoundNum.ToString());

        CleanGame();
        DrawMapCells();
        
        // player positions
        int positionPlayerOne = Random.Range(0, 2);
        int positionPlayerTwo = Random.Range(2, 4);
        
        players[0].Reset(playerPrefab,SpawnPositions[positionPlayerOne][0], SpawnPositions[positionPlayerOne][1]);
        players[1].Reset(playerPrefab, SpawnPositions[positionPlayerTwo][0], SpawnPositions[positionPlayerTwo][1]);
        
        players[0].Freeze();
        players[1].Freeze();
        
        yield return new WaitForSeconds(2);
        
        panels.Message("", false);
        UnlockGame();
    }
    
    private void OnEnable()
    {
        UnlockGame(); // unfreeze game when gameManager is enabled
    }

    private void OnDisable()
    {
        LockGame(); // freeze game when gameManager is disabled
    }
    
    /*
     * Function to destroy all gameObjects fire bomb and bonus
     */
    private void DestroyAllGameObjects() {
      foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire")) Destroy(fire);
      foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb")) Destroy(bomb);
      foreach (GameObject bonus in GameObject.FindGameObjectsWithTag("Bonus")) Destroy(bonus);
    }

}

