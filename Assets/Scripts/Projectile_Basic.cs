using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Basic : MonoBehaviour
{
    Rigidbody2D rigidbody2;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.magnitude > 50.0f)
        //{
        //    Destroy(gameObject);

        //}
    }

    public void launch(Vector2 vector, float force)
    {
        rigidbody2.AddForce(vector * force);

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        EnemyNormalController enemy = other.gameObject.GetComponent<EnemyNormalController>();


        if (enemy != null)
        {
            enemy.currentHealth = enemy.health - 2;
            Destroy(gameObject);
            
        }

        


        EnemyChimera chimera = other.gameObject.GetComponent<EnemyChimera>();

        //Debug.Log(chimera);
        if (chimera != null)
        {
            chimera.currentHealth = chimera.health - 2;
            Destroy(gameObject);
        }

        Destroy(gameObject);
        
    }
    
}
