using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    private static int roundNum = 3;
    private static int timeNum = 120;

    public GameObject TMPRoundNum;
    public GameObject TMPTimeNum;


    public 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeNum == 0)
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = timeNum.ToString();
        }

        if (roundNum == 0)
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = roundNum.ToString();
        }
    }


    public void upRoundNum()
    {
        if (roundNum < 99) roundNum++;
    }
    public void downRoundNum()
    {
        if (roundNum > 0) roundNum--;
    }

    public void upTimeNum()
    {
        if(timeNum < 240) timeNum++; // max 4 minutes
    }
    public void downTimeNum()
    {
        if(timeNum > 0) timeNum--;
    }


    public void startGame(){
        GameManager.roundMaxNum = roundNum;
        GameManager.winMaxNum = timeNum;
        SceneManager.LoadScene(1);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
