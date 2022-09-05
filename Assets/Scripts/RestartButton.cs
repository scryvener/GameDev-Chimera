using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called before the first frame update


    private UnityAction Restart;


    void Start()
    {
        Restart += Pressed;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Restart);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed()
    {
        SceneManager.LoadScene("MainScene");

        
    }
}
