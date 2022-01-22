using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public Player player;
    public GameObject bombGameObject;

    private string[] keyboard; 
    private float timer = 0;
    private float keyboardDelay = 1f;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        keyboard = player.GetKeyboard();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if( timer >= keyboardDelay ){
            if (Input.GetKey(keyboard[4])){
                Instantiate(bombGameObject, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), transform.rotation);
                timer = 0;
            }
            
        }

    }
}
