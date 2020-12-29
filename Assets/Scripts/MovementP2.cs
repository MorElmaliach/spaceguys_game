using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementP2 : MonoBehaviour
{
    public float speed = 10f;
    public const float original_speed = 10f;
    Vector2 dest = Vector2.zero;
    private bool notSpeedBuff = true;

    Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move closer to Destination
        GetComponent<Rigidbody2D>().AddForce(dest * speed);        
        if (GameObject.Find("gamemanager").GetComponent<GameManager>().isGameOver())
        {
            if (Input.GetKey(KeyCode.W) && valid(Vector2.up))
                dest = Vector2.up;
            else if (Input.GetKey(KeyCode.D) && valid(Vector2.right))
                dest = Vector2.right;
            else if (Input.GetKey(KeyCode.S) && valid(Vector2.down))
                dest = Vector2.down;
            else if (Input.GetKey(KeyCode.A) && valid(Vector2.left))
                dest = Vector2.left;
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.zero);
            }
        }
        // Animation Parameters
        GetComponent<Animator>().SetFloat("DirX", dest.x);
        GetComponent<Animator>().SetFloat("DirY", dest.y);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        GetComponent<Rigidbody2D>().rotation = 0;
        if (notSpeedBuff)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 10)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < -10)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            }
            if (GetComponent<Rigidbody2D>().velocity.y > 10)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
            }
            else if (GetComponent<Rigidbody2D>().velocity.y < -10)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -10);
            }
        }
    }

    public void Restart()
    {
        float randomX = Random.Range(2.5f, 29.0f);
        float randomY = Random.Range(2.5f, 29.0f);
        if ((randomX >= 12.9 || randomX <= 18.9) && (randomY >= 15.3 || randomY <= 18.9))
        {
            randomX = Random.Range(2.5f, 29.0f);
            randomY = Random.Range(2.5f, 29.0f);
        }
        transform.position = dest = new Vector2(randomX, randomY);

    }


    bool valid(Vector2 dir)
    {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }


    public void ChangeSpeed(float Speed, int Time, bool Modifier)
    {
        speed = speed + (Modifier ? speed * Speed : speed * -Speed)/100;
        notSpeedBuff = false;
        StartCoroutine(TimedEffect(Time));
    }


    public void FreezePlayer(int time)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        speed = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(TimedEffect(time));
    }

    IEnumerator TimedEffect(int Time)
    {
        yield return new WaitForSeconds(Time);

        speed = original_speed;
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        notSpeedBuff = true;
    }
}
