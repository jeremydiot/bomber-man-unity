using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    private static int maxRoundNum = 3;
    private static int maxTimeNum = 120;

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
        if (maxTimeNum == 0)
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = maxTimeNum.ToString();
        }

        if (maxRoundNum == 0)
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = maxRoundNum.ToString();
        }
    }


    public void upRoundNum()
    {
        if (maxRoundNum < 99) maxRoundNum++;
    }
    public void downRoundNum()
    {
        if (maxRoundNum > 0) maxRoundNum--;
    }

    public void upTimeNum()
    {
        if(maxTimeNum < 240) maxTimeNum++; // max 4 minutes
    }
    public void downTimeNum()
    {
        if(maxTimeNum > 0) maxTimeNum--;
    }


    public void startGame(){
        GameManager.maxRoundNum = maxRoundNum;
        GameManager.maxTimeNum = maxTimeNum;
        SceneManager.LoadScene(1);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
