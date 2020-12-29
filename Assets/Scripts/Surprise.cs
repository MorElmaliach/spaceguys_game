using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surprise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D co)
    {
        if (co.tag.Contains("player_1"))
        {
            gameObject.SetActive(false);
            GameObject.Find("gamemanager").GetComponent<GameManager>().ConsumeSurprise(true);
        }
        if (co.tag.Contains("player_2"))
        {
            gameObject.SetActive(false);
            GameObject.Find("gamemanager").GetComponent<GameManager>().ConsumeSurprise(false);
        }
    }

}

