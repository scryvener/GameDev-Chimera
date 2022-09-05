using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    //public PlayerController player;
    public float speed = .5f;

    public int currentHealth = 4;
    public int health { get { return currentHealth; } }
    

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Basic behavior is to chase after the player avatar

        //Rotate to face player
        GameObject player = GameObject.Find("Player");        

        Rigidbody2D playerbody = player.GetComponent<Rigidbody2D>();

        Vector3 target=playerbody.position;

        float x_transformed = target.x - rigidbody2d.position.x;
        float y_transformed = target.y - rigidbody2d.position.y;

        Vector3 target_direction = new Vector3(x_transformed, y_transformed,0);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target_direction);
        rigidbody2d.MoveRotation(targetRotation);

        //Move towards player

        Vector3 currentPosition = rigidbody2d.position;

        currentPosition = currentPosition + target_direction.normalized * speed * Time.deltaTime;

        rigidbody2d.MovePosition(currentPosition);

        if (currentHealth <= 0)
        {
            //pull player controller to access gold change functions

            PlayerController playerController = player.GetComponent<PlayerController>();
            //add function to check for unique behavior, and if those controllers exist, execute death functions

            SplitterController splitter = GetComponent<SplitterController>();

            if (splitter != null)
            {

                splitter.Death();

            }

            playerController.changeGold(10);
            Destroy(gameObject);
            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.changeHealth(-1);
        }

        

    }


}
