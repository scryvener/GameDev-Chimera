using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ProjectilePanel;
    public GameObject LaserPanel;
    public GameObject MissilePanel;
    public GameObject MobilityPanel;
    public GameObject SurvivalPanel;


    public GameObject Spawning;

    public GameObject player;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateProjectile()
    {
        returnToDefault();
        ProjectilePanel.SetActive(true);
    }

    public void ActivateLaser()
    {
        returnToDefault();
        LaserPanel.SetActive(true);
    }

    public void ActivateMissile()
    {
        returnToDefault();
        MissilePanel.SetActive(true);
    }

    public void ActivateMobility()
    {
        returnToDefault();
        MobilityPanel.SetActive(true);
    }

    public void ActivateSurvival()
    {
        returnToDefault();
        SurvivalPanel.SetActive(true);
    }

    void returnToDefault()
    {
        ProjectilePanel.SetActive(false);
        LaserPanel.SetActive(false);
        MissilePanel.SetActive(false);
        MobilityPanel.SetActive(false);
        SurvivalPanel.SetActive(false);
    }

    public void StartWave()
    {

        //Debug.Log("Start Wave button pressed");
        
        gameObject.SetActive(false);

        Spawning.SetActive(true);
        Spawning.GetComponent<SpawnController>().spawnWave = true;
        Spawning.GetComponent<SpawnController>().waveFinishedSpawning = false;
        player.GetComponent<PlayerController>().unpauseGame();
    }
}


