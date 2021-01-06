using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CandymanMoves : MonoBehaviourPun
{
    public float speed = 10f;
    public const float original_speed = 10f;
    Vector2 dest = Vector2.zero;
    private bool notSpeedBuff = true;
    public Sprite greenSprite;
    public Sprite yellowSprite;

    Vector2 startPosition;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        { 
            gameObject.GetComponent<SpriteRenderer>().sprite = yellowSprite;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 4, gameObject.transform.position.y,0);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = greenSprite;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 4, gameObject.transform.position.y, 0);
        }
        
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            _uiGo.gameObject.transform.position = this.gameObject.transform.position;
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        // Move closer to Destination
        GetComponent<Rigidbody2D>().AddForce(dest * speed);
            if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
                dest = Vector2.up;
            else if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
                dest = Vector2.right;
            else if (Input.GetKey(KeyCode.DownArrow) && valid(Vector2.down))
                dest = Vector2.down;
            else if (Input.GetKey(KeyCode.LeftArrow) && valid(Vector2.left))
                dest = Vector2.left;
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.zero);
            }

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

    void CalledOnLevelWasLoaded(int level)
    {
        //// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        //if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        //{
        //    transform.position = new Vector3(0f, 5f, 0f);
        //}
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

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
        speed = speed + (Modifier ? speed * Speed : speed * -Speed) / 100;
        notSpeedBuff = false;
        StartCoroutine(TimedEffect(Time));
    }


    public void FreezePlayer(int time)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        speed = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(TimedEffect(time));
    }

    IEnumerator TimedEffect(int Time)
    {
        yield return new WaitForSeconds(Time);

        speed = original_speed;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        notSpeedBuff = true;
    }
}