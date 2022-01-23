using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // provide acces to general game information
    public static GameManager Instance;

    // static games parameters and configuration
    public static int roundNum = 1;
    public static int winNum = 1;

    public static int colNum = 17;
    public static int rowNum = 13;

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

    public GameObject MenuPanel;
    public GameObject GamePanel;

    private TextMeshProUGUI TMPRoundNum;
    private TextMeshProUGUI TMPPlayerOneWinNum;
    private TextMeshProUGUI TMPPlayerTwoWinNum;
    private TextMeshProUGUI TMPPlayerOneHealth;
    private TextMeshProUGUI TMPPlayerTwoHealth;

    // game dynamique variables
    private Cell[][] groundCellsLayer = new Cell[rowNum][];
    public Cell[][] mapCellsLayer = new Cell[rowNum][];

    private Player[] players = new Player[]{
        new Player(1, new string[]{"z", "q", "s", "d", "space"}),
        new Player(2,  new string[]{"up", "left", "down", "right", "return"})
    };

    private int currentRoundNum = 1;

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
        SpawnPlayer();

        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);

        TMPRoundNum = GamePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneWinNum = GamePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoWinNum = GamePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneHealth = GamePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoHealth = GamePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
}

    // Update is called once per frame
    void Update(){

        if (Input.GetKey("escape"))
        {
            enabled = false;
            MenuPanel.SetActive(true);
            GamePanel.SetActive(false);
        }

            // display game info

            if (roundNum < 1) TMPRoundNum.text = currentRoundNum.ToString()+" / inf";
        else TMPRoundNum.text = currentRoundNum.ToString()+" / "+roundNum.ToString();

        if (winNum < 1)
        {
            TMPPlayerOneWinNum.text = "Win "+players[0].GetWinNum().ToString()+" / inf";
            TMPPlayerTwoWinNum.text = "Win "+players[1].GetWinNum().ToString()+" / inf";
        }
        else
        {
            TMPPlayerOneWinNum.text = "Win "+players[0].GetWinNum().ToString()+" / "+winNum.ToString();
            TMPPlayerTwoWinNum.text = "Win "+players[1].GetWinNum().ToString()+" / " + winNum.ToString();
        }

        TMPPlayerOneHealth.text = "Life "+ players[0].GetHealt().ToString();
        TMPPlayerTwoHealth.text = "Life "+ players[1].GetHealt().ToString();

        if (players[0].IsDead() || players[1].IsDead()) // if one player is dead
        {
            enabled = false; // stop update
            endRound();
        }
    }

    public void endRound()
    {
        Player alivePlayer = null;
        
        Player winPlayer = null;
        bool equality = false;

        foreach (Player player in players)
        {
            if (!player.IsDead())alivePlayer = player;
            if (winPlayer == null) winPlayer = player;
            else if (winPlayer.GetWinNum() < player.GetWinNum()) winPlayer = player;
            else if (winPlayer.GetWinNum() == player.GetWinNum()) equality = true;
        }

        if ((currentRoundNum >= roundNum && roundNum > 0) || (winPlayer.GetWinNum() >= winNum && winNum > 0))
        {
            /*Debug.Log("END ROUND !");
            Debug.Log("player win "+winPlayer.GetNumber().ToString());
            Debug.Log("player alive "+alivePlayer.GetNumber().ToString());
            if (equality) Debug.Log("EQUALITY !");*/
            enabled = false;
            MenuPanel.SetActive(true);
            GamePanel.SetActive(false);
            CleanGame();
        }
        else
        {
            currentRoundNum++;
            CleanGame();
            DrawBreakableGameCells();
            SpawnPlayer();
            enabled = true; // start update
        }
        
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
    private void SpawnPlayer(){

        int positionPlayerOne = Random.Range(0, 2);
        int positionPlayerTwo = Random.Range(2, 4);

        players[0].RebornAndDraw(PlayerPrefab, SpawnPositions[positionPlayerOne][0], SpawnPositions[positionPlayerOne][1]);
        players[1].RebornAndDraw(PlayerPrefab, SpawnPositions[positionPlayerTwo][0], SpawnPositions[positionPlayerTwo][1]);

        players[0].SetHealth(10);
        players[1].SetHealth(10);
    
        players[0].setEnemy(players[1]);
        players[1].setEnemy(players[0]);

    }



    private void CleanGame(){

        foreach(Player player in players)
        {
            player.KillAndErase();
        }

        foreach(GameObject fire in GameObject.FindGameObjectsWithTag("Fire"))
        {
            Destroy(fire);
        }

        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(fire);
        }

        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {
                Cell cell = mapCellsLayer[r][c];

                if (cell.IsErasable())
                {
                    cell.Erase();
                }

            }
        }
    }

    public void resumeBtn()
    {
        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
        this.enabled = true;
        
    }

    public void quitGameBtn()
    {
        SceneManager.LoadScene(0);
    }
}

