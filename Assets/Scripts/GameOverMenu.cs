using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public SpawnController spawn;


    void Awake()
    {
        Text[] textlist = GetComponentsInChildren<Text>();

        Text menutext = textlist[0];

        menutext.text = "You Have Died!\nYou made it to wave: " + spawn.waveCounter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
