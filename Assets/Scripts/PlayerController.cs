using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Text gameText;
    public Text healthText;
    public Text goldText;
    public GameObject pausePanel;
    public GameObject GameOverPanel;

    Weapons weapons;

    Rigidbody2D rigidbody2d;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    public int health;
    public bool healthUpgrade = false;
    public bool healthDynamicSuccesful = false;
    public int maxHealthUpgradeCost = 250;
    public Vector2 position { get { return rigidbody2d.position; } }
    public int maxHealth = 5;
    public float speed = 10.0f;
    public Vector2 firedirection;
    bool isInvincible = false;
    int invincibleTimer = 2;
    public bool isPaused = false;

    public bool dashEnabled = false;
    public int gold;

    

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        weapons = GetComponent<Weapons>();
        health = maxHealth;
        displayHealth();
        gold = 100;
        displayGold();

        gameObject.SetActive(false);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Up down left right movement
        displayHealth();
        displayGold();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))

        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();

        }
        Vector2 position = rigidbody2d.position;
        position = position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        //Health related
        if (isInvincible)
        {
            invincibleTimer = invincibleTimer - 1;
        }

        if (invincibleTimer == 0)
        {
            invincibleTimer = 2;
            isInvincible = false;
        }

        //dash ability
        if (Input.GetKeyDown("space"))
        {
            if (dashEnabled == true)
            {
                isInvincible = true;

                //pull mouse position

                Plane playerPlane = new Plane(Vector3.forward, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float ray_distance;

                playerPlane.Raycast(ray, out ray_distance);
                Vector3 mouse_position = ray.GetPoint(ray_distance);

                Vector2 dashPosition = mouse_position;//replace with mouse position

                //dashPosition = move * 5;
                rigidbody2d.MovePosition(dashPosition);
                isInvincible = false;
            }
        }



        //pausing the game
        if (Input.GetKeyDown("p"))
        {

            if (isPaused == false)
            {
                pauseGame();
            }
            else
            {
                unpauseGame();
            }

        }
        

    } 

    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
        }

        health = Mathf.Clamp(health + amount, 0, maxHealth);

        if (health == 0)
        {
            pauseGame();
            GameOverPanel.SetActive(true);

        }

    }

    public void changeGold(int amount)
    {
        gold += amount;
    }
    
    void displayHealth()
    {
        healthText.text = "Health: " + health.ToString() + "/"+ maxHealth.ToString();
    }

    public void displayGold()
    {
        goldText.text = "Gold: " + gold.ToString();
    }


    public void pauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;

        if (health > 0)
        {
            pausePanel.SetActive(true);
        }

    }

    public void unpauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pausePanel.SetActive(false);

        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

    }

    public void exitApp()
    {
        Application.Quit();
    }

    public void activateDash()
    {

        if (gold < 300)
        {
            gameText.text = "You Do Not Have Enough Gold!";
            gameText.GetComponent<FadeOut>().TextFadeOut();
            weapons.purchaseSuccesful = false;
        }
        else
        {
            if (dashEnabled == true)
            {
                gameText.text = "You have already purchased this!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                weapons.purchaseSuccesful = false;
            }
            else
            {
                gameText.text = "Purchase Succesful!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                dashEnabled = true;
                weapons.purchaseSuccesful = true;
                gold = gold - 300;
                displayGold();
            }
        }
    }

    public void increaseMaxHealth()
    {
        if (gold < maxHealthUpgradeCost)
        {
            gameText.text = "You Do Not Have Enough Gold!";
            gameText.GetComponent<FadeOut>().TextFadeOut();
            weapons.purchaseSuccesful = false;
        }
        else
        {
            
            gameText.text = "Purchase Succesful!";
            gameText.GetComponent<FadeOut>().TextFadeOut();
            healthDynamicSuccesful = true;
            gold = gold - maxHealthUpgradeCost;
            maxHealth += 1;
            displayHealth();
            displayGold();

            maxHealthUpgradeCost += 250;
            
        }
    }

    public void replenishHealth()
    {
        if (gold < 25)
        {
            gameText.text = "You Do Not Have Enough Gold!";
            gameText.GetComponent<FadeOut>().TextFadeOut();
            weapons.purchaseSuccesful = false;
        }
        else
        {
            if (health == maxHealth)
            {
                gameText.text = "You are already at maximum health!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                weapons.purchaseSuccesful = false;
            }
            else
            {
                gameText.text = "Purchase Succesful!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                weapons.purchaseSuccesful = true;
                healthUpgrade = true;
                gold = gold - 25;
                health = health + 1;
                displayHealth();
                displayGold();
            }
        }
    }

    public void replenishMaxHealth()//dynamic, need to update button everytime you open the screen
    {
        int cost = (maxHealth - health)*25;

        if (gold < cost)
        {
            gameText.text = "You Do Not Have Enough Gold!";
            gameText.GetComponent<FadeOut>().TextFadeOut();
            healthDynamicSuccesful = false;
        }
        else
        {
            if (health == maxHealth)
            {
                gameText.text = "You are already at maximum health!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                healthDynamicSuccesful = false;
            }
            else
            {
                gameText.text = "Purchase Succesful!";
                gameText.GetComponent<FadeOut>().TextFadeOut();
                healthDynamicSuccesful = true;
                gold = gold - cost;
                health = maxHealth;
                displayHealth();
                displayGold();

                
            }
        }

    }

    public void resetAll()
    {
        healthUpgrade = false;
        healthDynamicSuccesful = false;
        maxHealthUpgradeCost = 250;
        maxHealth = 5;
        speed = 10.0f;
        dashEnabled = false;
        gold = 100;
        displayGold();
        displayHealth();

    }




}
