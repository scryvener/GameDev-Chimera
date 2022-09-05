using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterController : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject splitterPrefab;
    Rigidbody2D rigidbody2d;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Death()
    {
        Instantiate(splitterPrefab, rigidbody2d.position + Vector2.up * .5f, Quaternion.identity);
        Instantiate(splitterPrefab, rigidbody2d.position + Vector2.down * .5f, Quaternion.identity);
    }

}
