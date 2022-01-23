using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Movement : MonoBehaviour
{

    public Player player;

    private string[] keyboard;
    private float speed = 10;

    public Canvas prefabCanvas;
    private Canvas instanciateCanvas;

    private TextMeshProUGUI TMPPlayerNum;
    

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        keyboard = player.GetKeyboard();
        instanciateCanvas = Instantiate(prefabCanvas);
        TMPPlayerNum = instanciateCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMPPlayerNum.text = player.GetNumber().ToString();
        //player.GetCanvas().transform.GetChild(0).gameObject.SetActive(false);
        //TMPPlayerNum =  player.GetCanvas().transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //TextMeshProUGUI text =  Instantiate(TMPPlayerNum);
    }

    // Update is called once per frame
    void Update()
    {

        float x = 0;
        float y = 0;

        if (Input.GetKey(keyboard[0])) y = 1;
        if (Input.GetKey(keyboard[1])) x = -1;
        if (Input.GetKey(keyboard[2])) y = -1;
        if (Input.GetKey(keyboard[3])) x = 1;

        if (Input.GetKey(keyboard[0]) && Input.GetKey(keyboard[2])) y = 0;
        if (Input.GetKey(keyboard[1]) && Input.GetKey(keyboard[3])) x = 0;

        TMPPlayerNum.gameObject.transform.position = gameObject.transform.position;

        if (player.GetCanMove()){
            transform.Translate(new Vector3(x, y) * speed * Time.deltaTime);
        }

    }


    private void OnDestroy()
    {
        try
        {
            Destroy(TMPPlayerNum.gameObject);
            Destroy(instanciateCanvas.gameObject);
        }
        catch (Exception) { }
        
    }
}
