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
    public GameObject FinishPanel;
    public GameObject MessagePanel;
    public GameObject PlayerOnePanel;
    public GameObject PlayerTwoPanel;

    private TextMeshProUGUI TMPRoundNum;
    private TextMeshProUGUI TMPPlayerOneWin;
    private TextMeshProUGUI TMPPlayerTwoWin;
    private TextMeshProUGUI TMPPlayerOneLife;
    private TextMeshProUGUI TMPPlayerTwoLife;
    private TextMeshProUGUI TMPPlayerOneBomb;
    private TextMeshProUGUI TMPPlayerTwoBomb;
    private TextMeshProUGUI TMPPlayerOneDistance;
    private TextMeshProUGUI TMPPlayerTwoDistance;

    // game dynamique variables
    private Cell[][] groundCellsLayer = new Cell[rowNum][];
    public Cell[][] mapCellsLayer = new Cell[rowNum][];

    private Player[] players = new Player[]{
        new Player(1, new string[]{"z", "q", "s", "d", "space"}),
        new Player(2,  new string[]{"up", "left", "down", "right", "return"})
    };

    private int currentRoundNum = 1;
    private int currentResetNum = 0;

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

        resetGame();

        GamePanel.SetActive(true);
        MenuPanel.SetActive(false);
        FinishPanel.SetActive(false);

        TMPRoundNum = GamePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        TMPPlayerOneWin = PlayerOnePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoWin = PlayerTwoPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneLife = PlayerOnePanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoLife = PlayerTwoPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneBomb = PlayerOnePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoBomb = PlayerTwoPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();

        TMPPlayerOneDistance = PlayerOnePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoDistance = PlayerTwoPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
}

    // Update is called once per frame
    void Update(){

        if (Input.GetKey("escape"))
        {
            enabled = false;
            lockPlayer();
            MenuPanel.SetActive(true);
        }

            // display game info

        if (roundNum < 1) TMPRoundNum.text = currentRoundNum.ToString()+" / inf";
        else TMPRoundNum.text = currentRoundNum.ToString()+" / "+roundNum.ToString();

        if (winNum < 1)
        {

            TMPPlayerOneWin.text = players[0].GetWinNum().ToString()+" / inf";
            TMPPlayerTwoWin.text = players[1].GetWinNum().ToString()+" / inf";
        }
        else
        {
            TMPPlayerOneWin.text = players[0].GetWinNum().ToString()+" / "+winNum.ToString();
            TMPPlayerTwoWin.text = players[1].GetWinNum().ToString()+" / " + winNum.ToString();
        }

        TMPPlayerOneLife.text = players[0].GetHealt().ToString();
        TMPPlayerTwoLife.text = players[1].GetHealt().ToString();

        if (players[0].isInfiniteImpact()) TMPPlayerOneDistance.text = "inf";
        else TMPPlayerOneDistance.text = players[0].GetMaxImpact().ToString();

        if (players[1].isInfiniteImpact()) TMPPlayerTwoDistance.text = "inf";
        else TMPPlayerTwoDistance.text = players[1].GetMaxImpact().ToString();


        TMPPlayerOneBomb.text = players[0].GetAvailableBomb().ToString()+" / "+ players[0].GetMaxBomb().ToString();
        TMPPlayerTwoBomb.text = players[1].GetAvailableBomb().ToString()+" / "+ players[1].GetMaxBomb().ToString();


        if (players[0].IsDead() || players[1].IsDead()) // if one player is dead
        {
            enabled = false;
            
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

            if (equality)
            {
                FinishPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "EQUALITY !";
            }
            else
            {
                FinishPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PLAYER "+winPlayer.GetNumber().ToString()+" WIN !";
            }
            
            enabled = false;
            MenuPanel.SetActive(true);
            FinishPanel.SetActive(true);

        }
        else
        {
            currentRoundNum++;
            resetGame();
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

    private void lockPlayer(Player player = null)
    {
        if(player == null)
        {
            players[0].SetCanMove(false);
            players[1].SetCanMove(false);
            players[0].SetCanShoot(false);
            players[1].SetCanShoot(false);
        }
        else
        {
            player.SetCanMove(false);
            player.SetCanShoot(false);
        }
    }

    private void unlockPlayer(Player player = null)
    {
        if (player == null)
        {
            players[0].SetCanMove(true);
            players[1].SetCanMove(true);
            players[0].SetCanShoot(true);
            players[1].SetCanShoot(true);
        }
        else
        {
            player.SetCanMove(true);
            player.SetCanShoot(true);
        }
    }

    private void resetGame()
    {

        MessagePanel.SetActive(true);
        MessagePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Round " + currentRoundNum.ToString();

        lockPlayer();

        InvokeRepeating("resetProcess", 0, 0.7f);

    }

    private void resetProcess()
    {
        currentResetNum++;

        CleanGame();
        DrawBreakableGameCells();
        SpawnPlayer();

        if(currentResetNum >= 5)
        {
            CancelInvoke();
            currentResetNum = 0;

            unlockPlayer();

            MessagePanel.SetActive(false);

            enabled = true;
        }
            
    }

    private void CleanGame(){

        foreach (Player player in players) player.KillAndErase();

        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bomb")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bonus")) Destroy(fire);

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
        this.enabled = true;
        unlockPlayer();
        
    }

    public void quitGameBtn()
    {
        SceneManager.LoadScene(0);
    }
}

