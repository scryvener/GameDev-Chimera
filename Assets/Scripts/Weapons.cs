using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    public GameObject projectilePrefab1;//projectiles
    public GameObject projectilePrefab2;//basic laser
    public GameObject projectilePrefab3;//basic missile

    public int weaponNum;//what gets inputted when you press the button

    public PlayerController player;
    Rigidbody2D rigidbody2d;

    public int activeWeapon;
    public List<int> unlockedWeapons = new List<int>();
    List<int> weaponCosts = new List<int>() { 0, 250, 500, 750, 1000, 250, 500, 1000, 1000 , 250, 500, 750, 750, 1000};
    public bool purchaseSuccesful = false;
    public int weaponcounter = 0;

    // Start is called before the first frame update

    


    void Awake()
    {
        player = GetComponent<PlayerController>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        activeWeapon = 0;//Default weapon on start is single shot, 0
        unlockedWeapons.Add(0);//default unlocked
        activeWeapon = unlockedWeapons[0];
    }

    // Update is called once per frame
    void Update()
    {

        

        //Rotate body to face where the mouse is for firing purposes


        Plane playerPlane = new Plane(Vector3.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float ray_distance;

        playerPlane.Raycast(ray, out ray_distance);
        Vector3 mouse_position = ray.GetPoint(ray_distance);

        float x_transformed = mouse_position.x - transform.position.x;
        float y_transformed = mouse_position.y - transform.position.y;

        //float deg_rotation = Mathf.Tan(y_transformed / x_transformed) * Mathf.Rad2Deg;

        Vector3 target_rotation = new Vector3(x_transformed, y_transformed,0);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target_rotation);//In 2d you rotate along the z axis, this aligns z accordingly so you don't rotate in more dimension

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30.0f * Time.deltaTime);

        rigidbody2d.MoveRotation(targetRotation);

        //Fire Command
        if (Input.GetMouseButtonDown(0)&&player.isPaused!=true)
        {

            if (activeWeapon == 0)
            {
                SingleShot(target_rotation);
            }

            if (activeWeapon == 1)
            {
                DoubleShot(target_rotation);
            }

            if (activeWeapon == 2)
            {
                TripleShot(target_rotation);
            }

            if (activeWeapon == 3)
            {
                QuadShot(target_rotation);
            }

            if (activeWeapon == 4)
            {
                SpreadShot(target_rotation, targetRotation);
            }

            if (activeWeapon == 5)
            {
                LaserBasic(target_rotation);
            }

            if (activeWeapon == 6)
            {
                LaserDouble(target_rotation, targetRotation);
            }

            if (activeWeapon == 7)
            {
                LaserPierce(target_rotation);
            }

            if (activeWeapon == 8)
            {
                LaserBurst(target_rotation);
            }

            if (activeWeapon == 9)
            {
                MissileBasic(target_rotation);
            }

            if (activeWeapon == 10)
            {
                MissileDouble(target_rotation);
            }

            if (activeWeapon == 11)
            {
                MissileTriple(target_rotation);
            }

            if (activeWeapon == 12)
            {
                MissileHoming(target_rotation);
            }

            if (activeWeapon == 13)
            {
                MissileAOE(target_rotation);
            }
        }

        

        if (Input.GetKeyDown("c"))
        {
            

            if (weaponcounter >= unlockedWeapons.Count-1)
            {
                weaponcounter = 0;
            }
            else
            {
                weaponcounter += 1;
            }

            activeWeapon = unlockedWeapons[weaponcounter];

        }

        //ProjectileCleanup

        ProjectileCleanup(false);


    }

    public void ProjectileCleanup(bool all)
    {
        if (all == true)
        {
            GameObject[] activeProjectiles = GameObject.FindGameObjectsWithTag("Projectiles");

            if (activeProjectiles.Length != 0)
            {
                foreach (GameObject projectile in activeProjectiles)
                {
                    Destroy(projectile);
                }
            }

        }
        else
        {
            GameObject[] activeProjectiles = GameObject.FindGameObjectsWithTag("Projectiles");


            if (activeProjectiles.Length != 0)
            {
                foreach (GameObject projectile in activeProjectiles)
                {
                    Rigidbody2D r = projectile.GetComponent<Rigidbody2D>();

                    ColliderDistance2D distance = r.Distance(player.GetComponent<PolygonCollider2D>());

                    if (distance.distance > 50)
                    {
                        Destroy(projectile);
                    }

                }
            }
        }
    }

    public void ActivateWeapon(int weaponNum)
    {
        if (player.gold < weaponCosts[weaponNum])
        {
            player.gameText.text = "You Do Not Have Enough Gold!";
            player.gameText.GetComponent<FadeOut>().TextFadeOut();
            purchaseSuccesful = false;
        }
        else
        {
            //Debug.Log(unlockedWeapons.Exists(x => x == weaponNum));
            //Debug.Log(unlockedWeapons.Exists(x => x != weaponNum));

            if (unlockedWeapons.Exists(x => x == weaponNum))//giving problems again
            {
                player.gameText.text = "You Have Already Purchased This!";
                player.gameText.GetComponent<FadeOut>().TextFadeOut();
                purchaseSuccesful = false;
            }
            else
            {

                unlockedWeapons.Add(weaponNum);
                player.changeGold(-weaponCosts[weaponNum]);
                //activeWeapon = weaponNum;

                player.gameText.text = "Purchase Succesful!";
                player.gameText.GetComponent<FadeOut>().TextFadeOut();

                purchaseSuccesful = true;
                player.displayGold();
            }
        }  
    }

    void ProjectileShot(Vector3 target_rotation, int spawnAngle,GameObject projectilePrefab, Rigidbody2D rigidbody2d)
    {
        Vector2 enemyDirection = new Vector2(target_rotation.x, target_rotation.y);
        Vector3 firedirection;
        firedirection = Quaternion.Euler(0, 0, spawnAngle) * target_rotation;

        Vector2 fire = new Vector2(0, 0);
        fire = firedirection;
        GameObject projectileObject = Instantiate(projectilePrefab, (rigidbody2d.position + fire.normalized * .5f), transform.rotation);

        Projectile_Basic projectile = projectileObject.GetComponent<Projectile_Basic>();
        projectile.launch(enemyDirection.normalized, 700);
    }

    void SpreadShotBasic(Vector3 target_rotation, int spawnAngle, GameObject projectilePrefab, Rigidbody2D rigidbody2d,int force)//can be for projectiles or others, just swap out the prefab
    {
        Vector3 firedirection;
        firedirection = Quaternion.Euler(0, 0, spawnAngle) * target_rotation.normalized;
        Vector2 fire = new Vector2(0, 0);
        fire = firedirection;
        Quaternion target = Quaternion.LookRotation(Vector3.forward, fire);
        GameObject projectileObject = Instantiate(projectilePrefab, (rigidbody2d.position + fire.normalized * .6f), target);

        Projectile_Basic projectile = projectileObject.GetComponent<Projectile_Basic>();
        Missile_Basic missile = projectileObject.GetComponent<Missile_Basic>();

        if (projectile != null)
        {
            projectile.launch(fire.normalized, force);
        }

        if (missile != null)
        {
            missile.launch(fire.normalized, force);
        }

    }

    void SingleShot(Vector3 target_rotation)
    {
        ProjectileShot(target_rotation,0,projectilePrefab1,rigidbody2d);
    }

    void DoubleShot(Vector3 target_rotation)
    {
        ProjectileShot(target_rotation, 30, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, -30, projectilePrefab1, rigidbody2d);
    }

    void TripleShot(Vector3 target_rotation)
    {
        ProjectileShot(target_rotation, 0, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, 75, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, -75, projectilePrefab1, rigidbody2d);
    }

    void QuadShot(Vector3 target_rotation)
    {
        ProjectileShot(target_rotation, 25, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, -25, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, 95, projectilePrefab1, rigidbody2d);
        ProjectileShot(target_rotation, -95, projectilePrefab1, rigidbody2d);
    }

    void SpreadShot(Vector3 target_rotation, Quaternion targetRotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);//original target direction to launch projectiles

        SpreadShotBasic(target_rotation, 25, projectilePrefab1, rigidbody2d,700);
        SpreadShotBasic(target_rotation, -25, projectilePrefab1, rigidbody2d,700);
        SpreadShotBasic(target_rotation, 55, projectilePrefab1, rigidbody2d,700);
        SpreadShotBasic(target_rotation, -55, projectilePrefab1, rigidbody2d,700);

        //Center Projectile
        ProjectileShot(target_rotation, 0, projectilePrefab1, rigidbody2d);
    }

    void LaserBasic(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab2, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Laser_Basic projectile = projectileObject.GetComponent<Laser_Basic>();
        projectile.launch(firedirection.normalized, 1400);
    }

    void LaserDouble(Vector2 target_rotation, Quaternion targetRotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);//original target direction to launch projectiles
        Vector3 firedirection1;
        Vector3 firedirection2;

        firedirection1 = Quaternion.Euler(0, 0, 30) * target_rotation;

        firedirection2 = Quaternion.Euler(0, 0, -30) * target_rotation;

        Vector2 fire1 = new Vector2(0, 0);
        Vector2 fire2 = new Vector2(0, 0);

        fire1 = firedirection1;
        fire2 = firedirection2;

        GameObject projectileObject1 = Instantiate(projectilePrefab2, (rigidbody2d.position + fire1.normalized * .5f), transform.rotation);
        GameObject projectileObject2 = Instantiate(projectilePrefab2, (rigidbody2d.position + fire2.normalized * .5f), transform.rotation);

        Laser_Basic projectile1 = projectileObject1.GetComponent<Laser_Basic>();
        projectile1.launch(firedirection.normalized, 1400);
        
        Laser_Basic projectile2 = projectileObject2.GetComponent<Laser_Basic>();
        projectile2.launch(firedirection.normalized, 1400);
    }

    void LaserPierce(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab2, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Laser_Basic projectile = projectileObject.GetComponent<Laser_Basic>();

        projectile.pierce = true;

        projectile.launch(firedirection.normalized, 1000);
    }

    void LaserBurst(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab2, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Laser_Basic projectile = projectileObject.GetComponent<Laser_Basic>();

        projectile.burst = true;

        projectile.launch(firedirection.normalized, 1000);
    }

    void MissileBasic(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab3, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Missile_Basic projectile = projectileObject.GetComponent<Missile_Basic>();

        projectile.launch(firedirection.normalized, 300);
    }

    void MissileDouble(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);

        SpreadShotBasic(target_rotation, 15, projectilePrefab3, rigidbody2d, 300);
        SpreadShotBasic(target_rotation, -15, projectilePrefab3, rigidbody2d, 300);
    }

    void MissileTriple(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);

        SpreadShotBasic(target_rotation, 25, projectilePrefab3, rigidbody2d, 300);
        SpreadShotBasic(target_rotation, -25, projectilePrefab3, rigidbody2d, 300);
        SpreadShotBasic(target_rotation, 0, projectilePrefab3, rigidbody2d, 300);
    }

    void MissileHoming(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab3, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Missile_Basic projectile = projectileObject.GetComponent<Missile_Basic>();

        projectile.homing = true;

        var distanceList = new List<float>();
        var distanceListClone = new List<float>();

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemies");
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        foreach (GameObject target in activeEnemies)
        {
            Collider2D collider = target.GetComponent<CircleCollider2D>();
            ColliderDistance2D distance = rigidbody2d.Distance(collider);
            distanceList.Add(distance.distance);
            distanceListClone.Add(distance.distance);
        }

        distanceListClone.Sort();

        int shortestIndex = distanceList.IndexOf(distanceListClone[0]);

        Debug.Log(shortestIndex);
        GameObject targeted = activeEnemies[shortestIndex];
        Debug.Log(targeted);

        projectile.LaunchHome(firedirection.normalized, 100,targeted);

    }

    void MissileAOE(Vector2 target_rotation)
    {
        Vector2 firedirection = new Vector2(target_rotation.x, target_rotation.y);
        GameObject projectileObject = Instantiate(projectilePrefab3, rigidbody2d.position + firedirection.normalized * 1f, transform.rotation);
        Missile_Basic projectile = projectileObject.GetComponent<Missile_Basic>();

        projectile.Aoe = true;

        projectile.launch(firedirection.normalized, 300);

    }

    public void resetAll()
    {
        activeWeapon = 0;
        unlockedWeapons.Clear();
        weaponcounter = 0;
    }

}
