using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Player player;

    private string[] keyboard;
    private float speed = 10;
    private bool canMove = true;

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        keyboard = player.keyboard;
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

        if (canMove) transform.Translate(new Vector3(x, y) * speed * Time.deltaTime);

    }

}
