using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_Basic : MonoBehaviour
{

    Rigidbody2D rigidbody2;

    public bool homing=false;
    public bool Aoe=false;
    public GameObject targetedEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (homing == true)
        {

            if (targetedEnemy != null) {
                //set speed
                float speed = 3f;

                Vector3 targetedPosition = targetedEnemy.GetComponent<Rigidbody2D>().position;
                //Debug.Log(targetedPosition);
                //rotate to face enemy targeted
                float x_transformed = targetedPosition.x - rigidbody2.position.x;
                float y_transformed = targetedPosition.y - rigidbody2.position.y;

                Vector3 target_direction = new Vector3(x_transformed, y_transformed, 0);

                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target_direction);
                rigidbody2.MoveRotation(targetRotation);

                Vector3 currentPosition = rigidbody2.position;

                currentPosition = currentPosition + target_direction.normalized * speed * Time.deltaTime;

                rigidbody2.MovePosition(currentPosition);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void launch(Vector2 vector, float force)
    {
        rigidbody2.AddForce(vector * force);

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        
        if (Aoe == true)
        {
            GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemies");

            List<GameObject> damageList = new List<GameObject>();
            

            foreach (GameObject e in activeEnemies)
            {
                Collider2D collider = e.GetComponent<CircleCollider2D>();
                ColliderDistance2D distance = rigidbody2.Distance(collider);
                if (distance.distance < 5)
                {
                    damageList.Add(e);
                }
            }

            foreach (GameObject d in damageList)
            {
                EnemyNormalController enemy = d.gameObject.GetComponent<EnemyNormalController>();

                if (enemy != null)
                {
                    enemy.currentHealth = enemy.health - 2;
                }
                
                EnemyChimera chimera = d.gameObject.GetComponent<EnemyChimera>();

                if (chimera != null)
                {
                    chimera.currentHealth = chimera.health - 2;
                }
            }
            
            Destroy(gameObject);
        }
        else
        {
            EnemyNormalController enemy = other.gameObject.GetComponent<EnemyNormalController>();

            if (enemy != null)
            {
                enemy.currentHealth = enemy.health - 3;
                Destroy(gameObject);
            }

            EnemyChimera chimera = other.gameObject.GetComponent<EnemyChimera>();

            if (chimera != null)
            {
                chimera.currentHealth = chimera.health - 3;
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }

        //Add behavior for AOE explosion
    }

    public void LaunchHome(Vector2 vector,float force, GameObject targeted)
    {

        rigidbody2.AddForce(vector * force);
        targetedEnemy = targeted;

        
    }
    

}
