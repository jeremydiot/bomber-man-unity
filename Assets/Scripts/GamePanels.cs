using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/**
 * Manage game panels
 */
public class GamePanels : MonoBehaviour
{
    
    public GameObject MenuPanel;
    public GameObject InfoPanel;
    public GameObject GamePanel;
    public GameObject FinishPanel;
    
    public GameObject PlayersCameraPanel;

    private GameObject escapeInfoGameObject;
    
    private TextMeshProUGUI TMPRoundNum;
    private TextMeshProUGUI TMPTimeNum;
    private TextMeshProUGUI TMPPlayerOneWin;
    private TextMeshProUGUI TMPPlayerTwoWin;
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
        GamePanel.SetActive(true);
        
        MenuPanel.SetActive(false);
        FinishPanel.SetActive(false);
        InfoPanel.SetActive(false);

        escapeInfoGameObject = GamePanel.transform.GetChild(0).gameObject;

        TMPRoundNum = GamePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TMPTimeNum = GamePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneWin = GamePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneBomb = GamePanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneDistance = GamePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoWin = GamePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoBomb = GamePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoDistance = GamePanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>();

        TMPTimeNum.text = "";
        TMPRoundNum.text = "";
        
        TMPPlayerOneNumber = PlayersCameraPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMPPlayerOneNumber.gameObject.SetActive(false);
        TMPPlayerTwoNumber = PlayersCameraPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TMPPlayerTwoNumber.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // open menu
        if (Input.GetKey("escape") && !MenuPanel.activeSelf && !once)
        {
            once = true;
            MenuPanel.SetActive(true);
            
            GameManager.Instance.enabled = false;

            Task.Delay(250).ContinueWith(t =>
            {
                once = false;
            });
            
            
        }
        // close menu
        else if(Input.GetKey("escape") && MenuPanel.activeSelf && !once)
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

        if (GameManager.MaxRoundNum < 1) TMPRoundNum.text = GameManager.Instance.currentRoundNum.ToString()+" / inf";
        else TMPRoundNum.text = GameManager.Instance.currentRoundNum.ToString()+" / "+GameManager.MaxRoundNum.ToString();
        
        TMPPlayerOneWin.text = GameManager.Instance.players[0].winNum.ToString();
        TMPPlayerTwoWin.text = GameManager.Instance.players[1].winNum.ToString();

        if (GameManager.Instance.players[0].IsInfiniteDistance()) TMPPlayerOneDistance.text = "inf";
        else TMPPlayerOneDistance.text = GameManager.Instance.players[0].maxDistance.ToString();

        if (GameManager.Instance.players[1].IsInfiniteDistance()) TMPPlayerTwoDistance.text = "inf";
        else TMPPlayerTwoDistance.text = GameManager.Instance.players[1].maxDistance.ToString();


        TMPPlayerOneBomb.text = GameManager.Instance.players[0].availableBomb.ToString()+" / "+ GameManager.Instance.players[0].maxBomb.ToString();
        TMPPlayerTwoBomb.text = GameManager.Instance.players[1].availableBomb.ToString()+" / "+ GameManager.Instance.players[1].maxBomb.ToString();

        if (GameManager.Instance.players[0].GetInstantiateGameObject() != null)
        {
            if(!TMPPlayerOneNumber.IsActive())TMPPlayerOneNumber.gameObject.SetActive(true);
            TMPPlayerOneNumber.transform.position = GameManager.Instance.players[0].GetInstantiateGameObject().transform.position;
        }
        else
        {
            if(TMPPlayerOneNumber.IsActive())TMPPlayerOneNumber.gameObject.SetActive(false);
        }
        
        if (GameManager.Instance.players[1].GetInstantiateGameObject() != null)
        {
            if(!TMPPlayerTwoNumber.IsActive())TMPPlayerTwoNumber.gameObject.SetActive(true);
            TMPPlayerTwoNumber.transform.position = GameManager.Instance.players[1].GetInstantiateGameObject().transform.position;
        }
        else
        {
            if(TMPPlayerTwoNumber.IsActive())TMPPlayerTwoNumber.gameObject.SetActive(false);
        }
    }

    /**
     * Finish message
     */
    public void Finish(string text)
    {
        FinishPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        FinishPanel.SetActive(true);
    }
    
    /**
     * Game message
     */
    public void Message(string text = "" , bool status = true)
    {
        InfoPanel.SetActive(status);
        InfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }

    public void resumeBtn()
    {
        GameManager.Instance.enabled = true;
        MenuPanel.SetActive(false);
    }

    public void quitGameBtn()
    {
        SceneManager.LoadScene(0);
    }
}
