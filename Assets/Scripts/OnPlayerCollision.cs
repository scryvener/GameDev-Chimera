using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.changeHealth(-1);
        }

        //SendMessageUpwards("OnCollisionEnter2D", other);
    }
}
