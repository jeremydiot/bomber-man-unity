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
    public static int roundMaxNum = 1;
    public static int winMaxNum = 1;

    public static int colNum = 17;
    public static int rowNum = 13;

    private static int breakableWallNum = 40;
    private static int infiniteDistanceBonusNum = 4;
    private static int moreBombBonus = 4;   
    private static int moreDistanceBonus = 6;

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
    public Cell[][] mapCellsLayer = new Cell[rowNum][];

    private Player[] players = new Player[]{
        new Player(1, new string[]{"z", "q", "s", "d", "space"}),
        new Player(2, new string[]{"up", "left", "down", "right", "return"})
    };

    private int currentRoundNum = 1;
    private int currentResetNum = 0;

    private void Awake(){
        Instance = this;

        //init cells layers
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

    // Start is called before the first frame update
    void Start(){

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

        if (roundMaxNum < 1) TMPRoundNum.text = currentRoundNum.ToString()+" / inf";
        else TMPRoundNum.text = currentRoundNum.ToString()+" / "+roundMaxNum.ToString();

        if (winMaxNum < 1)
        {

            TMPPlayerOneWin.text = players[0].winNum.ToString()+" / inf";
            TMPPlayerTwoWin.text = players[1].winNum.ToString()+" / inf";
        }
        else
        {
            TMPPlayerOneWin.text = players[0].winNum.ToString()+" / "+winMaxNum.ToString();
            TMPPlayerTwoWin.text = players[1].winNum.ToString()+" / " + winMaxNum.ToString();
        }

        TMPPlayerOneLife.text = players[0].health.ToString();
        TMPPlayerTwoLife.text = players[1].health.ToString();

        if (players[0].isInfiniteDistance()) TMPPlayerOneDistance.text = "inf";
        else TMPPlayerOneDistance.text = players[0].maxDistance.ToString();

        if (players[1].isInfiniteDistance()) TMPPlayerTwoDistance.text = "inf";
        else TMPPlayerTwoDistance.text = players[1].maxDistance.ToString();


        TMPPlayerOneBomb.text = players[0].availableBomb.ToString()+" / "+ players[0].maxBomb.ToString();
        TMPPlayerTwoBomb.text = players[1].availableBomb.ToString()+" / "+ players[1].maxBomb.ToString();


        if (players[0].IsDead() || players[1].IsDead()) // if one player is dead
        {
            enabled = false;
            
            endRound();
        }
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

    public void endRound()
    {
        Player alivePlayer = null;
        Player winPlayer = null;
        bool equality = false;

        foreach (Player player in players)
        {
            if (!player.IsDead())alivePlayer = player;
            if (winPlayer == null) winPlayer = player;
            else if (winPlayer.winNum < player.winNum) winPlayer = player;
            else if (winPlayer.winNum == player.winNum) equality = true;
        }

        if ((currentRoundNum >= roundMaxNum && roundMaxNum > 0) || (winPlayer.winNum >= winMaxNum && winMaxNum > 0))
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


    private void spawnPlayer(){

        int positionPlayerOne = Random.Range(0, 2);
        int positionPlayerTwo = Random.Range(2, 4);

        players[0].reset();
        players[1].reset();

        players[0].ResuscitAndDraw(PlayerPrefab, SpawnPositions[positionPlayerOne][0], SpawnPositions[positionPlayerOne][1]);
        players[1].ResuscitAndDraw(PlayerPrefab, SpawnPositions[positionPlayerTwo][0], SpawnPositions[positionPlayerTwo][1]);
    
        players[0].setEnemy(players[1]);
        players[1].setEnemy(players[0]);

    }

    private void lockPlayer(Player player = null)
    {
        if(player == null)
        {
            players[0].canMove = false;
            players[1].canMove = false;
            players[0].canShoot = false;
            players[1].canShoot = false;
        }
        else
        {
            player.canShoot = false;
            player.canMove = false;
        }
    }

    private void unlockPlayer(Player player = null)
    {
        if (player == null)
        {
            players[0].canMove = true;
            players[1].canMove = true;
            players[0].canShoot = true;
            players[1].canShoot = true;
        }
        else
        {
            player.canMove = true;
            player.canShoot = true;
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

        cleanGame();
        drawMapCells();
        spawnPlayer();

        if(currentResetNum >= 5)
        {
            CancelInvoke();
            currentResetNum = 0;
            unlockPlayer();
            MessagePanel.SetActive(false);
            enabled = true;
        }
            
    }

    private void cleanGame(){

        foreach (Player player in players)player.KillAndErase();

        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bomb")) Destroy(fire);
        foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Bonus")) Destroy(fire);

        for (int r = 0; r < rowNum; r++)
        {
            for (int c = 0; c < colNum; c++)
            {
                mapCellsLayer[r][c].Erase();
                mapCellsLayer[r][c].bonusType = Cell.BonusType.none;
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

