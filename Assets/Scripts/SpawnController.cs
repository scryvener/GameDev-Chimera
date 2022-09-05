using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    // Start is called before the first frame update

    
    public GameObject menu;
    public GameObject player;
    public Text gameText;
    public Text textCounter;
    GameObject[] activeSpawns;
    public bool spawnWave = false;
    int nextwavecounter = 0;
    public int waveCounter = 0;
    int spawnSelectionNum;
    int typeSelectionNum;

    int currSpawnNum=0;
    public bool waveFinishedSpawning = false;
    int waveDelay = 0;

    //define variables needed to control spawning
    int spawnPerWave;
    float range1;
    float range2;
    float range3;
    float range4;
    float range5;
    int maxWave;

   



    void Start()
    {

        textCounter.text = "Current Wave: 0";
        //Debug.Log(waveCounter);
        spawnPerWave = 10;
        range1 = 0.065f;
        range2 = 0.265f;
        range3 = 0.465f;
        range4 = 0.485f;
        range5 = 0.935f;
        maxWave = 50;

}

    void Update()
    {
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemies");
        //Debug.Log(waveCounter);
        textCounter.text = "Current Wave: " + (waveCounter).ToString();
        //Debug.Log(textCounter.text);
        if (activeEnemies.Length == 0 && waveFinishedSpawning == true)
        {

            gameText.text = "Wave completed!";
            gameText.GetComponent<FadeOut>().TextFadeOut();

            player.GetComponent<Weapons>().ProjectileCleanup(true);


            menu.SetActive(true);
            gameObject.SetActive(false);
            //player.GetComponent<PlayerController>().pauseGame();

        }



        if (spawnWave == true)
        {
            if (waveCounter == maxWave)
            {
                gameText.text = "Game Completed!";
            }
            else
            {

                gameObject.SetActive(true);
                //Debug.Log("Spawning Enemies");
                
                //pull all available spawn sites.
                gameText.text = "";
                activeSpawns = GameObject.FindGameObjectsWithTag("SpawnController");

                //pull all available enemys.
                Object[] enemyList = Resources.LoadAll("Prefabs/Enemies", typeof(GameObject));


                if (currSpawnNum < spawnPerWave)
                {
                    if (waveDelay == 15)
                    {
                        //select where to spawn from
                        if (activeSpawns.Length == 1) { spawnSelectionNum = 0; }
                        else { spawnSelectionNum = Random.Range(0, activeSpawns.Length); }
                        GameObject spawnSelected = activeSpawns[spawnSelectionNum];

                        //select enemy to spawn randomly

                        float selectProb = Random.Range(0.0f, 1.0f);
                        if (selectProb <= range1)
                        {
                            typeSelectionNum = 0;
                        }
                        if (selectProb > range1 && selectProb <= range2)
                        {
                            typeSelectionNum = 1;
                        }
                        if (selectProb > range2 && selectProb <= range3)
                        {
                            typeSelectionNum = 2;
                        }
                        if (selectProb > range3 && selectProb <= range4)
                        {
                            typeSelectionNum = 3;
                        }
                        if (selectProb > range4 && selectProb<=range5)
                        {
                            typeSelectionNum = 4;
                        }
                        if (selectProb > range5)
                        {
                            typeSelectionNum = 5;
                        }

                        GameObject enemySelected = enemyList[typeSelectionNum] as GameObject;

                        Spawn(enemySelected, spawnSelected);

                        currSpawnNum += 1;
                        waveDelay = 0;

                    }
                    else
                    {
                        waveDelay += 1;
                    }

                }
                else
                {
                    waveCounter += 1;
                    //Debug.Log(waveCounter);
                    spawnWave = false;
                    waveFinishedSpawning = true;
                    currSpawnNum = 0;

                    if (waveCounter % 3 == 0 && waveCounter != 0)
                    {
                        AdjustSpawnNum();
                    }

                    if (waveCounter % 5 == 0 && waveCounter != 0)
                    {
                        AdjustSpawnComp();
                    }

                }

            }
        }

    }

    void Spawn(GameObject enemySelected, GameObject spawnSelected)//pass in wave counter to tell which combination of enemies to spawn
        {
            GameObject selectedspawn = Instantiate(enemySelected, spawnSelected.GetComponent<Transform>().position, Quaternion.identity);
        }

    void AdjustSpawnNum()//increase difficulty by adding more spawns per wave
    {
        if (waveCounter < 15)
        {
            spawnPerWave += Random.Range(0, 10);
        }

        if (waveCounter>=15 && waveCounter < 30)
        {
            spawnPerWave += Random.Range(0, 35);
        }

        if (waveCounter >= 30)
        {
            spawnPerWave += Random.Range(0, 75);
        }
    }

    void AdjustSpawnComp()//increases difficulty by weighting away from the normal spawns
    {
     
        //Convert to base percentages
        float per1 = range1 - 0;//armed
        float per2 = range2 - range1;//chunky
        float per3 = range3 - range2;//speedy
        float per4 = range4 - range3;//chimera
        float per5 = range5 - range4;//normal
        float per6 = 1 - range5;//splitter

        //Adjust percentages

        per5 -= .05f;//set amount

        per1 += .05f / 5;
        //Debug.Log(per1);
        per2 += .05f / 5;
        //Debug.Log(per2);
        per3 += .05f / 5;
        //Debug.Log(per3);

        per4 += .05f / 5;
        per5 += .05f / 5;
        //Debug.Log(per4);

        //check that all percentages add up to 1, if not, do nothing and give a debug warning

        if (per1 + per2 + per3 + per4 + per5 +per6== 1)
        {
            range1 = per1;
            //Debug.Log(range1);
            range2 = per1 + per2;
            range3 = per1 + per2 + per3;
            range4 = per1 + per2 + per3 + per4;
            range5 = per1 + per2 + per3 + per4 + per5;
        }
        else
        {
            Debug.Log("Maximum Difficulty Reached");//allow for more dynamic adjustment in the future
        }

    }

    public void resetAll()
    {
        spawnPerWave = 10;
        range1 = 0.065f;
        range2 = 0.265f;
        range3 = 0.465f;
        range4 = 0.485f;
        range5 = 0.935f;
        maxWave = 50;
        spawnWave = false;
        nextwavecounter = 0;
        waveCounter = 0;
        currSpawnNum = 0;
        waveDelay = 0;
        waveFinishedSpawning = false;
    }

}
