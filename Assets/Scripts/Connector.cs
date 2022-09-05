using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    // Start is called before the first frame update

    void awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //SendMessageUpwards("takeDamage", SendMessageOptions.RequireReceiver);
    }

}

