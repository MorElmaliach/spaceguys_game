using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform[] waypoints;
    int cur = 0;
    public float speed = 0.3f;
    Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name == "pacman")
        {
            GameObject.Find("gamemanager").GetComponent<GameManager>().Death();
            
        }

    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    void FixedUpdate()
    {
        if (GameObject.Find("gamemanager").GetComponent<GameManager>().isGameOver())
        {
            // Waypoint not reached yet? then move closer
            if (transform.position != waypoints[cur].position)
            {
                Vector2 p = Vector2.MoveTowards(transform.position,
                                                waypoints[cur].position,
                                                speed);
                GetComponent<Rigidbody2D>().MovePosition(p);
            }
            // Waypoint reached, select next one
            else cur = (cur + 1) % waypoints.Length;

            // Animation
            Vector2 dir = waypoints[cur].position - transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
    }

}
