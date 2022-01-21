using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    private float timerDestroy = 1f;
    public  GameObject fireGameObject;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);        
        Invoke("instantiateFire", 0.99f);
    }

    public void instantiateFire(){
        Instantiate(fireGameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), gameObject.transform.rotation);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
