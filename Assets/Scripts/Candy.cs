using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag.Contains("player_1"))
        {
            Destroy(gameObject);
            GameObject.Find("gamemanager").GetComponent<GameManager>().IncrementScore(true);
        }
        if (co.tag.Contains("player_2"))
        {
            Destroy(gameObject);
            GameObject.Find("gamemanager").GetComponent<GameManager>().IncrementScore(false);
        }
    }
}
