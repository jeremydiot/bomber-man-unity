using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{

    public Canvas prefabCanvas;
    private Canvas instanciateCanvas;
    private TextMeshProUGUI TMPPlayerHealth;
    private string message = "Life ";

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        instanciateCanvas = Instantiate(prefabCanvas);

        if (player.GetNumber() == 1){
            instanciateCanvas.transform.GetChild(1).gameObject.SetActive(false);
            instanciateCanvas.transform.GetChild(3).gameObject.SetActive(false);

            TMPPlayerHealth = instanciateCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        }
        else if (player.GetNumber() == 2)
        {
            instanciateCanvas.transform.GetChild(0).gameObject.SetActive(false);
            instanciateCanvas.transform.GetChild(2).gameObject.SetActive(false);

            TMPPlayerHealth = instanciateCanvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }

        TMPPlayerHealth.text = message + player.GetHealt().ToString();


    }

    // Update is called once per frame
    void Update()
    {
        TMPPlayerHealth.text = message + player.GetHealt().ToString();
        if (player.GetHealt() <= 0)
        {
            player.Erase();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            player.DownHealth();
        }
    }

}
