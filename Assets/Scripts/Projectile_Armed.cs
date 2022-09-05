using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Armed : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2;

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {

    }


    public void Launch(Vector3 vector, float force)
    {

        //Debug.Log(rigidbody2);
        rigidbody2.AddForce(vector * force);

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.changeHealth(-1);
            Destroy(gameObject);
        }

        if(player==null)
        {
            Destroy(gameObject);
        }
        
    }
}



