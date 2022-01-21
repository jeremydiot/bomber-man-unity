using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static string[] keyboardOne = new string[] {"z","q","s","d"};
    public static string[] keyboardTwo = new string[] {"up","left","down","right","enter"};

    public float speed = 10;
    public int playerNum = 1;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float x = 0;
        float y = 0;

        string[] currentKeyboard = keyboardOne;
        if(playerNum > 1)currentKeyboard = keyboardTwo;

        if (Input.GetKey(currentKeyboard[0])) y = 1;
        if (Input.GetKey(currentKeyboard[1])) x = -1;
        if (Input.GetKey(currentKeyboard[2])) y = -1;
        if (Input.GetKey(currentKeyboard[3])) x = 1;

        if (Input.GetKey(currentKeyboard[0]) && Input.GetKey(currentKeyboard[2])) y = 0;
        if (Input.GetKey(currentKeyboard[1]) && Input.GetKey(currentKeyboard[3])) x = 0;

        if (canMove) transform.Translate(new Vector3(x, y) * speed * Time.deltaTime);

    }

}
