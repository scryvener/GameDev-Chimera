using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Text temp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp.text = "Hello World";
    }
}
