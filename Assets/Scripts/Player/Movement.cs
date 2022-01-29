using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Movement : MonoBehaviour
{

    public Player player;

    private string[] keyboard;
    private float speed = 5f;
    
    private void Start()
    {
        keyboard = player.GetKeyboard();
    }

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
        
    

        transform.Translate(new Vector3(x, y) * speed * Time.deltaTime);
    }
}