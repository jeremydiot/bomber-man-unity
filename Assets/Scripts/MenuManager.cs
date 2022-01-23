using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    private static int roundNum = 1;
    private static int winNum = 1;

    public GameObject TMPRoundNum;
    public GameObject TMPWinNum;


    public 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (winNum == 0)
        {
            TMPWinNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPWinNum.GetComponent<TextMeshProUGUI>().text = winNum.ToString();
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

    public void upWinNum()
    {
        if(winNum < 99) winNum++;
    }
    public void downWinNum()
    {
        if(winNum > 0) winNum--;
    }


    public void startGame(){
        GameManager.roundNum = roundNum;
        GameManager.winNum = winNum;
        SceneManager.LoadScene(1);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
