using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public static string keyboardOne = "space"; 
    public static string keyboardTwo = "return";

    public GameObject bombGameObject;

    public int playerNum = 1;

    private float timer = 0;
    private float keyboardDelay = 1f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        string currentKeyboard = keyboardOne;
        if (playerNum > 1) currentKeyboard = keyboardTwo;

        timer += Time.deltaTime;
        if( timer >= keyboardDelay ){
            if (Input.GetKey(currentKeyboard)){
                Instantiate(bombGameObject, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), transform.rotation);
                timer = 0;
            }
            
        }

    }
}
