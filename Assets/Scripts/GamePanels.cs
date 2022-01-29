using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePanels : MonoBehaviour
{
    
    public GameObject MenuPanel;
    
    public GameObject GameHeadPanel;
    public GameObject GameFootPanel;
    public GameObject GameMiddlePanel;
    
    public GameObject PlayerOneGameSidePanel;
    public GameObject PlayerTwoGameSidePanel;
    
    public GameObject FinishPanel;
    
    public GameObject PlayersCameraPanel;

    private TextMeshProUGUI TMPRoundNum;
    private TextMeshProUGUI TMPTimeNum;
    private TextMeshProUGUI TMPPlayerOneWin;
    private TextMeshProUGUI TMPPlayerTwoWin;
    private TextMeshProUGUI TMPPlayerOneLife;
    private TextMeshProUGUI TMPPlayerTwoLife;
    private TextMeshProUGUI TMPPlayerOneBomb;
    private TextMeshProUGUI TMPPlayerTwoBomb;
    private TextMeshProUGUI TMPPlayerOneDistance;
    private TextMeshProUGUI TMPPlayerTwoDistance;
    
    private TextMeshProUGUI TMPPlayerTwoNumber;
    private TextMeshProUGUI TMPPlayerOneNumber;

    private bool once = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayersCameraPanel.SetActive(true);
        
        MenuPanel.SetActive(false);
        FinishPanel.SetActive(false);
        GameMiddlePanel.SetActive(false);
        
        GameHeadPanel.SetActive(true);
        GameFootPanel.SetActive(true);
        PlayerOneGameSidePanel.SetActive(true);
        PlayerTwoGameSidePanel.SetActive(true);
        
        TMPRoundNum = GameHeadPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TMPTimeNum = GameHeadPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        
        TMPTimeNum.text = "";
        TMPRoundNum.text = "";
        
        TMPPlayerOneWin = PlayerOneGameSidePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoWin = PlayerTwoGameSidePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        // TMPPlayerOneLife = PlayerOneGameSidePanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        // TMPPlayerTwoLife = PlayerTwoGameSidePanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneBomb = PlayerOneGameSidePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoBomb = PlayerTwoGameSidePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneDistance = PlayerOneGameSidePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoDistance = PlayerTwoGameSidePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();

        TMPPlayerOneNumber = PlayersCameraPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneNumber.gameObject.SetActive(false);
        TMPPlayerTwoNumber = PlayersCameraPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoNumber.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape") && !MenuPanel.activeSelf && !once)
        {
            once = true;
            MenuPanel.SetActive(true);
            GameManager.Instance.enabled = false;
            GameManager.Instance.lockGame();
            
            Task.Delay(250).ContinueWith(t =>
            {
                once = false;
            });
            
            
        }else if(Input.GetKey("escape") && MenuPanel.activeSelf && !once)
        {
            once = true;
            MenuPanel.SetActive(false);
            resumeBtn();
            
            Task.Delay(250).ContinueWith(t =>
            {
                once = false;
            });
        }
        
        TMPTimeNum.text = ((int)GameManager.Instance.currentTime).ToString();

        if (GameManager.maxRoundNum < 1) TMPRoundNum.text = GameManager.Instance.currentRoundNum.ToString()+" / inf";
        else TMPRoundNum.text = GameManager.Instance.currentRoundNum.ToString()+" / "+GameManager.maxRoundNum.ToString();

        TMPPlayerOneWin.text = GameManager.Instance.players[0].winNum.ToString();
        TMPPlayerTwoWin.text = GameManager.Instance.players[1].winNum.ToString();

        // TMPPlayerOneLife.text = GameManager.Instance.players[0].health.ToString();
        // TMPPlayerTwoLife.text = GameManager.Instance.players[1].health.ToString();

        if (GameManager.Instance.players[0].isInfiniteDistance()) TMPPlayerOneDistance.text = "inf";
        else TMPPlayerOneDistance.text = GameManager.Instance.players[0].maxDistance.ToString();

        if (GameManager.Instance.players[1].isInfiniteDistance()) TMPPlayerTwoDistance.text = "inf";
        else TMPPlayerTwoDistance.text = GameManager.Instance.players[1].maxDistance.ToString();


        TMPPlayerOneBomb.text = GameManager.Instance.players[0].availableBomb.ToString()+" / "+ GameManager.Instance.players[0].maxBomb.ToString();
        TMPPlayerTwoBomb.text = GameManager.Instance.players[1].availableBomb.ToString()+" / "+ GameManager.Instance.players[1].maxBomb.ToString();

        if (GameManager.Instance.players[0].GetInstanciateGameObject() != null)
        {
            TMPPlayerOneNumber.gameObject.SetActive(true);
            TMPPlayerOneNumber.transform.position = GameManager.Instance.players[0].GetInstanciateGameObject().transform.position;
        }
        else
        {
            TMPPlayerOneNumber.gameObject.SetActive(false);
        }
        
        if (GameManager.Instance.players[1].GetInstanciateGameObject() != null)
        {
            TMPPlayerTwoNumber.gameObject.SetActive(true);
            TMPPlayerTwoNumber.transform.position = GameManager.Instance.players[1].GetInstanciateGameObject().transform.position;
        }
        else
        {
            TMPPlayerTwoNumber.gameObject.SetActive(false);
        }
    }

    public void Finish(string text)
    {
        FinishPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        FinishPanel.SetActive(true);
    }
    
    public void Message(string text = "" , bool status = true)
    {
        GameMiddlePanel.SetActive(status);
        GameMiddlePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        
    }

    public void resumeBtn()
    {
        GameManager.Instance.enabled = true;
        GameManager.Instance.unlockGame();
        MenuPanel.SetActive(false);
    }

    public void quitGameBtn()
    {
        SceneManager.LoadScene(0);
    }
}
