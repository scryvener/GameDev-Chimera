using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Laser_Basic : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pierce = false;
    public bool burst = false;

    public GameObject childLaser;
    

    Rigidbody2D rigidbody2;
    BoxCollider2D collider;
    public Vector2 originalVelocity;
    public Vector2 velocityStorage;
    int minMagnitude = 5;
    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        originalVelocity = rigidbody2.velocity;
        if (originalVelocity.x == 0 && originalVelocity.y==0)
        {
            originalVelocity = velocityStorage;
        }
    }

    public void launch(Vector2 vector, float force)
    {
        rigidbody2.AddForce(vector * force);
        velocityStorage = vector;

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.GetComponent<Collider2D>());

        Collider2D enemyCollider = other.gameObject.GetComponent<Collider2D>();

        //CircleCollider2D enemyCollider = other.gameObject.GetComponent<CircleCollider2D>();
        EnemyNormalController enemy = other.gameObject.GetComponent<EnemyNormalController>();
        EnemyChimera enemyChimera = other.gameObject.GetComponent<EnemyChimera>();

        TilemapCollider2D wall = other.gameObject.GetComponent<TilemapCollider2D>();

        //Debug.Log(enemyCollider);
        
        if (pierce == true || burst == true)
        {
            if (pierce == true)
            {
                //pierce behavior
                //rigidbody2.WakeUp();
                Physics2D.IgnoreCollision(collider, enemyCollider);
                    
                ApplyDamage(enemy, enemyChimera);

                SpawnChild(velocityStorage, gameObject, enemyCollider, rigidbody2, 0);
                Destroy(gameObject);
                //launch(originalVelocity, 300);
                //velocityStorage = originalVelocity;

            }

            if (burst == true)
            {
                //burst behavior

                Physics2D.IgnoreCollision(collider, enemyCollider);
                ApplyDamage(enemy, enemyChimera);

                SpawnChild(velocityStorage, childLaser, enemyCollider, rigidbody2, 25);
                SpawnChild(velocityStorage, childLaser, enemyCollider, rigidbody2, -25);
                SpawnChild(velocityStorage, childLaser, enemyCollider, rigidbody2, 55);
                SpawnChild(velocityStorage, childLaser, enemyCollider, rigidbody2, -55);
                Destroy(gameObject);
            }
        }
        else
        {
            //normal laser behavior

            ApplyDamage(enemy, enemyChimera);
            Destroy(gameObject);
        }                
        

        if (wall != null)
        {
            Destroy(gameObject);
        }
    }

    void ApplyDamage(EnemyNormalController enemy, EnemyChimera enemyChimera)
    {
        if (enemy != null)
        {
            enemy.currentHealth = enemy.health - 1;
        }

        if (enemyChimera != null)
        {
            enemyChimera.currentHealth = enemyChimera.health - 1;
        }
    }

    void SpawnChild(Vector2 originalVelocity, GameObject childLaser, Collider2D enemyCollider, Rigidbody2D rigidbody2, int rotateAngle)
    {
        Vector3 childDirection;
        childDirection = Quaternion.Euler(0, 0, rotateAngle) * originalVelocity.normalized;
        Vector2 child = new Vector2(0, 0);
        child = childDirection;
        Quaternion childRotation = Quaternion.LookRotation(Vector3.forward, child);
        GameObject projectileObject = Instantiate(childLaser, rigidbody2.position + child.normalized * 1f, childRotation);

        BoxCollider2D childCollider = projectileObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(childCollider, enemyCollider);

        Laser_Basic childProjectile = projectileObject.GetComponent<Laser_Basic>();
        childProjectile.launch(child.normalized, 1000);
    }
}
