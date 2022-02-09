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
    

    // Update is called once per frame
    void Update()
    {
        // display chronometer
        if (maxTimeNum == 0)
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPTimeNum.GetComponent<TextMeshProUGUI>().text = maxTimeNum.ToString();
        }

        // display round num
        if (maxRoundNum == 0)
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = "inf";
        }
        else
        {
            TMPRoundNum.GetComponent<TextMeshProUGUI>().text = maxRoundNum.ToString();
        }
    }

    /**
     * Up max round number action
     */
    public void upRoundNum()
    {
        if (maxRoundNum < 99) maxRoundNum++;
    }
    
    /**
     * Down max round number action
     */
    public void downRoundNum()
    {
        if (maxRoundNum > 0) maxRoundNum--;
    }

    /**
     * Up max time action
     */
    public void upTimeNum()
    {
        if(maxTimeNum < 240) maxTimeNum++; // max 4 minutes
    }
    
    /**
     * Up max time action
     */
    public void downTimeNum()
    {
        if(maxTimeNum > 0) maxTimeNum--;
    }

    /**
     * Load game scene
     */
    public void startGame(){
        GameManager.MaxRoundNum = maxRoundNum;
        GameManager.MaxTimeNum = maxTimeNum;
        SceneManager.LoadScene(1);
    }

    /**
     * Exit application
     */
    public void quitButton()
    {
        Application.Quit();
    }
}
