using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Armed : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectilePrefab;
    Rigidbody2D rigidbody2d;

    public int firetimer;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        firetimer = 100;

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");

        Rigidbody2D playerbody = player.GetComponent<Rigidbody2D>();

        Vector3 target = playerbody.position;

        float x_transformed = target.x - rigidbody2d.position.x;
        float y_transformed = target.y - rigidbody2d.position.y;

        Vector3 target_direction = new Vector3(x_transformed, y_transformed, 0);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target_direction);
        rigidbody2d.MoveRotation(targetRotation);


        Vector3 curposition = rigidbody2d.position;


        if(player.GetComponent<PlayerController>().isPaused == false)
        {
            if(firetimer == 100)
        {
                GameObject projectileObject = Instantiate(projectilePrefab, curposition + target_direction * .25f, targetRotation);

                //Debug.Log(projectileObject);
                Projectile_Armed projectile = projectileObject.GetComponent<Projectile_Armed>();
                //Debug.Log(projectile);

                if (projectile != null)
                {
                    projectile.Launch(target_direction.normalized, 700);

                }
                firetimer = 0;

            }
        else
        {
                firetimer += 1;

            }
        }
    }
}

