using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Mic mic;
    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            mic.SetTimeToPlay(false);
            mic.SetTimeToPrepare(false);
            text.text = "Winner "+ mic.getCurrentPlayer().name;
        }
    }
}
